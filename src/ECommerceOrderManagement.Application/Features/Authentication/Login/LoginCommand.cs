using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Authentication.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;