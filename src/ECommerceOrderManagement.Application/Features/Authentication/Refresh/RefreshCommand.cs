using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Authentication.Refresh;

public sealed record RefreshCommand(string RefreshToken): ICommand<RefreshResponse>;