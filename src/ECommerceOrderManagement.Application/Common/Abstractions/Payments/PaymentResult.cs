namespace ECommerceOrderManagement.Application.Common.Abstractions.Payments;

public sealed record PaymentResult(bool IsSuccess, string? ProviderPaymentId, string? FailureReason);