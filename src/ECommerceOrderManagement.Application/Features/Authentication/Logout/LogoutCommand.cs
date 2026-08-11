using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Authentication.Logout;

public sealed record LogoutCommand(string RefreshToken) : ICommand;