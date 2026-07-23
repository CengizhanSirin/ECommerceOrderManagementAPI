using ECommerceOrderManagement.Application.Common.Results;
using MediatR;

namespace ECommerceOrderManagement.Application.Common.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;