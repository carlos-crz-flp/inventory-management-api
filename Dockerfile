# =========================
# Build Stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar archivos de proyecto
COPY ["Inventory.Api/Inventory.Api.csproj", "Inventory.Api/"]
COPY ["Inventory.Application/Inventory.Application.csproj", "Inventory.Application/"]
COPY ["Inventory.Domain/Inventory.Domain.csproj", "Inventory.Domain/"]
COPY ["Inventory.Infrastructure/Inventory.Infrastructure.csproj", "Inventory.Infrastructure/"]
COPY ["Inventory.SharedKernel/Inventory.SharedKernel.csproj", "Inventory.SharedKernel/"]

# Restaurar dependencias
RUN dotnet restore "Inventory.Api/Inventory.Api.csproj"

# Copiar el resto del código
COPY . .

# Publicar
WORKDIR "/src/Inventory.Api"

RUN dotnet publish "Inventory.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# Runtime Stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Instalar bash (requerido por wait-for-it.sh)
RUN apt-get update \
    && apt-get install -y --no-install-recommends bash \
    && rm -rf /var/lib/apt/lists/*

# Copiar aplicación publicada
COPY --from=build /app/publish .

# Copiar scripts
COPY wait-for-it.sh .
COPY docker-entrypoint.sh .

# Permisos
RUN chmod +x wait-for-it.sh \
    && chmod +x docker-entrypoint.sh

ENTRYPOINT ["./docker-entrypoint.sh"]