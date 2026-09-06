namespace ECommerceOrderManagement.Application.Common.Abstractions.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync(string payload, CancellationToken cancellationToken = default);
}