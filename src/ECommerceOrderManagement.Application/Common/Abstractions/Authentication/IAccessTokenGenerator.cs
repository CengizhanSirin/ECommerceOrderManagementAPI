namespace ECommerceOrderManagement.Application.Common.Abstractions.Authentication;

public interface IAccessTokenGenerator
{
    AccessTokenResult Generate(Guid userId,string email,IReadOnlyCollection<string> roles);
}