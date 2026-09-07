FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ECommerceOrderManagementAPI.slnx ./
COPY Directory.Packages.props ./

COPY src/ECommerceOrderManagement.API/ECommerceOrderManagement.API.csproj src/ECommerceOrderManagement.API/
COPY src/ECommerceOrderManagement.Application/ECommerceOrderManagement.Application.csproj src/ECommerceOrderManagement.Application/
COPY src/ECommerceOrderManagement.Domain/ECommerceOrderManagement.Domain.csproj src/ECommerceOrderManagement.Domain/
COPY src/ECommerceOrderManagement.Infrastructure/ECommerceOrderManagement.Infrastructure.csproj src/ECommerceOrderManagement.Infrastructure/
COPY src/ECommerceOrderManagement.Persistence/ECommerceOrderManagement.Persistence.csproj src/ECommerceOrderManagement.Persistence/
COPY tests/ECommerceOrderManagement.IntegrationTests/ECommerceOrderManagement.IntegrationTests.csproj tests/ECommerceOrderManagement.IntegrationTests/

RUN dotnet restore ECommerceOrderManagementAPI.slnx

COPY . .

RUN dotnet publish src/ECommerceOrderManagement.API/ECommerceOrderManagement.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerceOrderManagement.API.dll"]