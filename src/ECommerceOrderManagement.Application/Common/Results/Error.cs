namespace ECommerceOrderManagement.Application.Common.Results;

public sealed record Error
{
    private Error(string code, string description, ErrorType type, IReadOnlyCollection<ValidationError>? validationErrors = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));

        ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

        Code = code;
        Description = description;
        Type = type;

        ValidationErrors = validationErrors?.ToArray() ?? Array.Empty<ValidationError>();
    }

    private Error()
    {
        Code = string.Empty;
        Description = string.Empty;
        Type = ErrorType.Failure;
        ValidationErrors = Array.Empty<ValidationError>();
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    public static Error None { get; } = new();

    public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);


    public static Error Validation(IReadOnlyCollection<ValidationError> validationErrors)
    {
        ArgumentNullException.ThrowIfNull(validationErrors);

        if (validationErrors.Count == 0)
        {
            throw new ArgumentException("At least one validation error must be provided.", nameof(validationErrors));
        }

        return new Error("Validation.General", "One or more validation errors occurred.", ErrorType.Validation, validationErrors);
    }

    public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);

    public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);

    public static Error Unauthorized(string code, string description) => new(code, description, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string description) => new(code, description, ErrorType.Forbidden);
}
