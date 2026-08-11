using ECommerceOrderManagement.Application.Common.Abstractions.Identity;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Authentication.Register;

internal sealed class RegisterCommandHandler(IIdentityService identityService) : ICommandHandler<RegisterCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var creationResult = await identityService.CreateUserAsync(
             command.FirstName,
             command.LastName,
             command.Email,
             command.Password,
             cancellationToken);

        if (!creationResult.Succeeded)
        {
            var duplicateEmailError = creationResult.Errors.FirstOrDefault(error =>
                    error.Code.Equals("DuplicateEmail", StringComparison.OrdinalIgnoreCase)
                    ||
                    error.Code.Equals("DuplicateUserName", StringComparison.OrdinalIgnoreCase));


            if (duplicateEmailError is not null)
            {
                return Result<RegisterResponse>.Failure(AuthenticationErrors.EmailAlreadyExists(command.Email));
            }

            throw new InvalidOperationException($"User registration failed. Identity errors:" +
                $"{string.Join("; ", creationResult.Errors.Select(error => $"{error.Code}: {error.Description}"))}");
        }
        return Result<RegisterResponse>.Success(new RegisterResponse(creationResult.UserId!.Value, command.Email.Trim().ToLowerInvariant()));
    }
}