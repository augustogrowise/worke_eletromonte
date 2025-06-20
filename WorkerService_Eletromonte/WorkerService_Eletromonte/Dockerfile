# Etapa de runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o .csproj e restaura dependências
COPY WorkerService_Eletromonte/WorkerService_Eletromonte.csproj ./WorkerService_Eletromonte/
RUN dotnet restore ./WorkerService_Eletromonte/WorkerService_Eletromonte.csproj

# Copia o restante dos arquivos
COPY WorkerService_Eletromonte/. ./WorkerService_Eletromonte/
WORKDIR /src/WorkerService_Eletromonte
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publicação
FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerService_Eletromonte.dll"]
