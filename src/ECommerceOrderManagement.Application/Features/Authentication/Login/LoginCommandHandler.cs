using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Identity;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Authentication.Login;

internal sealed class LoginCommandHandler(IIdentityService identityService, IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenStore refreshTokenStore) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var authenticationResult = await identityService.AuthenticateAsync(command.Email, command.Password, cancellationToken);

        if (authenticationResult.Status != IdentityAuthenticationStatus.Succeeded)
        {
            return Result<LoginResponse>.Failure(AuthenticationErrors.InvalidCredentials());
        }

        var userId = authenticationResult.UserId!.Value;

        var accessToken = accessTokenGenerator.Generate(userId, authenticationResult.Email!, authenticationResult.Roles);

        var refreshToken = refreshTokenGenerator.Generate();

        await refreshTokenStore.AddAsync(userId, refreshToken.TokenHash, refreshToken.ExpiresAtUtc, cancellationToken);

        return Result<LoginResponse>.Success(new LoginResponse(accessToken.Token, accessToken.ExpiresAtUtc, refreshToken.Token, refreshToken.ExpiresAtUtc));
    }
}