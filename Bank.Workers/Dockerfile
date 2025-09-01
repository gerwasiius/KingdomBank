# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Bank.Workers/Bank.Workers.csproj", "Bank.Workers/"]
COPY ["Bank.App/Bank.App.csproj", "Bank.App/"]
COPY ["Bank.Domain/Bank.Domain.csproj", "Bank.Domain/"]
COPY ["Bank.Infrastructure/Bank.Infrastructure.csproj", "Bank.Infrastructure/"]
COPY ["Bank.Shared/Bank.Shared.csproj", "Bank.Shared/"]
RUN dotnet restore "./Bank.Workers/Bank.Workers.csproj"
COPY . .
WORKDIR "/src/Bank.Workers"
RUN dotnet build "./Bank.Workers.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bank.Workers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bank.Workers.dll"]