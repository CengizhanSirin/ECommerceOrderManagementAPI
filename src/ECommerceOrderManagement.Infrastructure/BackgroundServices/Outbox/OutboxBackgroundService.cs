using ECommerceOrderManagement.Application.Common.Abstractions.Messaging;
using ECommerceOrderManagement.Application.Common.Abstractions.Outbox;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Payments.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECommerceOrderManagement.Infrastructure.BackgroundServices.Outbox;

internal sealed class OutboxBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<OutboxProcessingOptions> options,
    ILogger<OutboxBackgroundService> logger,
    TimeProvider timeProvider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessagesAsync(stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {

                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred while processing outbox messages.");
            }



            try
            {
                await Task.Delay(TimeSpan.FromSeconds(options.Value.PollingIntervalSeconds), stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }

    private async Task ProcessPendingMessagesAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var messagePublisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

        var outboxQueries = scope.ServiceProvider.GetRequiredService<IOutboxQueries>();

        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var messages = await outboxQueries.GetPendingAsync(
            options.Value.BatchSize,
            options.Value.MaxRetryCount,
            cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                if (message.Type == nameof(PaymentSucceededDomainEvent))
                {
                    await messagePublisher.PublishAsync(message.Payload, cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported outbox message type: {message.Type}");
                }

                await outboxRepository.MarkAsProcessedAsync(message.Id, timeProvider.GetUtcNow().UtcDateTime, cancellationToken);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Outbox message {MessageId} processed successfully.", message.Id);
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {

                logger.LogError(exception, "Failed to process outbox message {MessageId}.", message.Id);


                await outboxRepository.MarkAsFailedAsync(
                    message.Id,
                    exception.Message,
                    cancellationToken);

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}