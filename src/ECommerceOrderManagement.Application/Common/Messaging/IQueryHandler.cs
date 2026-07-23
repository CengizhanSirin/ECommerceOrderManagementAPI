using ECommerceOrderManagement.Application.Common.Results;
using MediatR;

namespace ECommerceOrderManagement.Application.Common.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;