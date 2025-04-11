FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/EasyPoll.API.csproj", "src/API/"]
COPY ["src/Infra/EasyPoll.Infra.csproj", "src/Infra/"]
COPY ["src/Core/EasyPoll.Core.csproj", "src/Core/"]
RUN dotnet restore "src/API/EasyPoll.API.csproj"
COPY . .
WORKDIR "/src/src/API"
RUN dotnet build "EasyPoll.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EasyPoll.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EasyPoll.API.dll"]
