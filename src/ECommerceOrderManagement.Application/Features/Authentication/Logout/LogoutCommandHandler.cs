using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Authentication.Logout;

internal sealed class LogoutCommandHandler(IRefreshTokenGenerator refreshTokenGenerator, IRefreshTokenStore refreshTokenStore, TimeProvider timeProvider)
    : ICommandHandler<LogoutCommand>
{
    public async Task<Result> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var tokenHash = refreshTokenGenerator.ComputeHash(command.RefreshToken);

        var storedRefreshToken = await refreshTokenStore.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (storedRefreshToken is null || storedRefreshToken.RevokedAtUtc is not null)
        {
            return Result.Success();
        }

        await refreshTokenStore.RevokeAsync(storedRefreshToken.Id, storedRefreshToken.UserId, timeProvider.GetUtcNow().UtcDateTime, cancellationToken);

        return Result.Success();
    }
}