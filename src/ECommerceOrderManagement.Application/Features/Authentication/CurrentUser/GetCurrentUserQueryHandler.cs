using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Authentication.CurrentUser;

internal sealed class GetCurrentUserQueryHandler(ICurrentUser currentUser) : IQueryHandler<GetCurrentUserQuery, CurrentUserResponse>
{
    public Task<Result<CurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var response = new CurrentUserResponse(currentUser.UserId, currentUser.Email, currentUser.Roles);

        return Task.FromResult(Result<CurrentUserResponse>.Success(response));
    }
}