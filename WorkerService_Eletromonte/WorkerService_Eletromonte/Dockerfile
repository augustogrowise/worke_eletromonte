# Etapa de runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o .csproj e restaura
COPY *.csproj ./
RUN dotnet restore

# Copia tudo e compila
COPY . ./
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publicação
FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerService_Eletromonte.dll"]
