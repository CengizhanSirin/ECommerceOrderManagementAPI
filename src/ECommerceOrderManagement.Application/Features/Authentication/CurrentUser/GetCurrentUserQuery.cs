using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Authentication.CurrentUser;

public sealed record GetCurrentUserQuery : IQuery<CurrentUserResponse>;