using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Identity;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Authentication.Refresh;

internal sealed class RefreshCommandHandler(IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenStore refreshTokenStore, IIdentityService identityService,
    IAccessTokenGenerator accessTokenGenerator, TimeProvider timeProvider) : ICommandHandler<RefreshCommand, RefreshResponse>
{
    public async Task<Result<RefreshResponse>> Handle(RefreshCommand command, CancellationToken cancellationToken)
    {
        var tokenHash = refreshTokenGenerator.ComputeHash(command.RefreshToken);

        var storedRefreshToken = await refreshTokenStore.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (storedRefreshToken is null)
        {
            return Result<RefreshResponse>.Failure(AuthenticationErrors.InvalidRefreshToken());
        }

        var now = timeProvider.GetUtcNow().UtcDateTime;

        if (storedRefreshToken.RevokedAtUtc is not null || storedRefreshToken.ExpiresAtUtc <= now)
        {
            return Result<RefreshResponse>.Failure(AuthenticationErrors.InvalidRefreshToken());
        }

        var user = await identityService.GetUserByIdAsync(storedRefreshToken.UserId, cancellationToken);

        if (user is null || !user.IsActive)
        {
            return Result<RefreshResponse>.Failure(AuthenticationErrors.InvalidRefreshToken());
        }

        var accessToken = accessTokenGenerator.Generate(user.UserId, user.Email, user.Roles);

        var newRefreshToken = refreshTokenGenerator.Generate();

        var rotated = await refreshTokenStore.RotateAsync(
                storedRefreshToken.Id,
                user.UserId,
                newRefreshToken.TokenHash,
                newRefreshToken.ExpiresAtUtc,
                now,
                cancellationToken);

        if (!rotated)
        {
            return Result<RefreshResponse>.Failure(AuthenticationErrors.InvalidRefreshToken());
        }

        return Result<RefreshResponse>.Success(new RefreshResponse(accessToken.Token, accessToken.ExpiresAtUtc, newRefreshToken.Token, newRefreshToken.ExpiresAtUtc));
    }
}