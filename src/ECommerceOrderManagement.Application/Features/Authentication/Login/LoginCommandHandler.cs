using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Identity;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Authentication.Login;

internal sealed class LoginCommandHandler(IIdentityService identityService, IAccessTokenGenerator accessTokenGenerator) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var authenticationResult = await identityService.AuthenticateAsync(command.Email, command.Password, cancellationToken);

        if (authenticationResult.Status != IdentityAuthenticationStatus.Succeeded)
        {
            return Result<LoginResponse>.Failure(AuthenticationErrors.InvalidCredentials());
        }

        var accessToken = accessTokenGenerator.Generate(authenticationResult.UserId!.Value, authenticationResult.Email!, authenticationResult.Roles);

        return Result<LoginResponse>.Success(new LoginResponse(accessToken.Token, accessToken.ExpiresAtUtc));
    }
}