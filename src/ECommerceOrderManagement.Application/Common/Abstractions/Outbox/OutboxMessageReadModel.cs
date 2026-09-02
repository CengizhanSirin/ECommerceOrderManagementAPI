namespace ECommerceOrderManagement.Application.Common.Abstractions.Outbox;

public sealed record OutboxMessageReadModel(Guid Id, string Type, string Payload, DateTime OccurredOnUtc, int RetryCount);