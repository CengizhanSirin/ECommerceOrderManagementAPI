namespace ECommerceOrderManagement.Application.Common.Abstractions.Authentication;

public interface IRefreshTokenGenerator
{
    RefreshTokenResult Generate();

    string ComputeHash(string token);
}