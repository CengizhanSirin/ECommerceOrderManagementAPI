namespace ECommerceOrderManagement.Application.Common.Exceptions;

public sealed class ConcurrencyConflictException : Exception
{
    public ConcurrencyConflictException() : base("A concurrency conflict occurred while saving changes.")
    {
    }

    public ConcurrencyConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }
}