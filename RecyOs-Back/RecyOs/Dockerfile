FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Argument pour la configuration de build (Debug ou Release)
ARG BUILD_CONFIGURATION="Release"

COPY ["./RecyOs.csproj", "RecyOs/"]
RUN dotnet restore "RecyOs/RecyOs.csproj"

WORKDIR "/src/RecyOs"
COPY . .

RUN dotnet build "RecyOs.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "RecyOs.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copier le fichier XML généré dans le répertoire /app si le fichier existe
COPY --from=build /app/build/RecyOs.xm* /app/
ARG BUILD_ENVIRONEMENT="Production"

# Configuration des variables d'environnement pour ASP.NET Core
ENV ASPNETCORE_ENVIRONMENT=$BUILD_ENVIRONEMENT
ENV ASPNETCORE_URLS="https://+:443"

ENTRYPOINT ["dotnet", "RecyOs.dll"]