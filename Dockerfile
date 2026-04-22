FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# CAMBIA "TactiqApi.csproj" por el nombre exacto de tu archivo si es distinto
COPY ["TactiqApi.csproj", "./"]
RUN dotnet restore "TactiqApi.csproj"
COPY . .
RUN dotnet build "TactiqApi.csproj" -c Release -o /app/build

# Publicar la app
FROM build AS publish
RUN dotnet publish "TactiqApi.csproj" -c Release -o /app/publish

# Configuración final para que corra en Render
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# CAMBIA "TactiqApi.dll" por el nombre de tu dll si es distinto
ENTRYPOINT ["dotnet", "TactiqApi.dll"]