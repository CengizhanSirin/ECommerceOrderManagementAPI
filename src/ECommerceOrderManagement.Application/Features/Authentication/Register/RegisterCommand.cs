using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Authentication.Register;

public sealed record RegisterCommand(string FirstName, string LastName, string Email, string Password, string ConfirmPassword) : ICommand<RegisterResponse>;