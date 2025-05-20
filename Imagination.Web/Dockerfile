FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Imagination.Web/Imagination.Web.csproj", "Imagination.Web/"]
COPY ["Imagination.Application/Imagination.Application.csproj", "Imagination.Application/"]
COPY ["Imagination.Domain/Imagination.Domain.csproj", "Imagination.Domain/"]
COPY ["Imagination.Shared/Imagination.Shared.csproj", "Imagination.Shared/"]
COPY ["Imagination.Infrastructure/Imagination.Infrastructure.csproj", "Imagination.Infrastructure/"]
COPY ["Imagination.Persistence/Imagination.Persistence.csproj", "Imagination.Persistence/"]
RUN dotnet restore "./Imagination.Web/Imagination.Web.csproj"
COPY . .
WORKDIR "/src/Imagination.Web"
RUN dotnet build "./Imagination.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Imagination.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Imagination.Web.dll"]