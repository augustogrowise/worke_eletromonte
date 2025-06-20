# Imagem base para runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Imagem para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o .csproj para restaurar dependências
COPY WorkerService_Eletromonte/*.csproj ./WorkerService_Eletromonte/
RUN dotnet restore ./WorkerService_Eletromonte/WorkerService_Eletromonte.csproj

# Copia tudo e compila
COPY . .
WORKDIR /src/WorkerService_Eletromonte
RUN dotnet build WorkerService_Eletromonte.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publica o app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish WorkerService_Eletromonte.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final: runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerService_Eletromonte.dll"]
