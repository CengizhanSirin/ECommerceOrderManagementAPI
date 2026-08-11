using ECommerceOrderManagement.API.Common.Exceptions;
using ECommerceOrderManagement.API.Common.Extensions;
using ECommerceOrderManagement.Application.DependencyInjection;
using ECommerceOrderManagement.Infrastructure.DependencyInjection;
using ECommerceOrderManagement.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication().AddSwaggerGenExt();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExt();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();