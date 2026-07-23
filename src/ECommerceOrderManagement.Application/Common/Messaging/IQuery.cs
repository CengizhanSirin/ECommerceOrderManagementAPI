using ECommerceOrderManagement.Application.Common.Results;
using MediatR;

namespace ECommerceOrderManagement.Application.Common.Messaging;

public interface IQuery<TResponse>: IRequest<Result<TResponse>>;