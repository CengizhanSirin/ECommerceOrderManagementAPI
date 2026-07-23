namespace ECommerceOrderManagement.Application.Common.Results;

public sealed record ValidationError(string PropertyName, string ErrorCode, string ErrorMessage);