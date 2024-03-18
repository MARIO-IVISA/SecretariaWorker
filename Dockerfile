#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Secretaria.WorkerService/Secretaria.WorkerService.csproj", "Secretaria.WorkerService/"]
COPY ["Secretaria.Application/Secretaria.Application.csproj", "Secretaria.Application/"]
COPY ["Secretaria.Domain/Secretaria.Domain.csproj", "Secretaria.Domain/"]
COPY ["Secretaria.Core/Secretaria.Core.csproj", "Secretaria.Core/"]
COPY ["Secretaria.Infrastructure/Secretaria.Infrastructure.csproj", "Secretaria.Infrastructure/"]
RUN dotnet restore "./Secretaria.WorkerService/Secretaria.WorkerService.csproj"
COPY . .
WORKDIR "/src/Secretaria.WorkerService"
RUN dotnet build "./Secretaria.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Secretaria.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Secretaria.WorkerService.dll"]