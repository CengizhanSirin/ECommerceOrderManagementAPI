namespace ECommerceOrderManagement.Infrastructure.Authentication;

internal sealed class RefreshTokenOptions
{
    public const string SectionName = "RefreshToken";

    public int ExpirationDays { get; set; }
}