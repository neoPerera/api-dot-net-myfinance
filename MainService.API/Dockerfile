# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# 👇 Timezone Setup
ENV TZ=Asia/Colombo
RUN apt-get update && \
    apt-get install -y tzdata && \
    ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
    echo $TZ > /etc/timezone
    
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MainService.API/MainService.API.csproj", "MainService.API/"]
COPY ["MainService.APPLICATION/MainService.APPLICATION.csproj", "MainService.APPLICATION/"]
COPY ["MainService.CORE/MainService.CORE.csproj", "MainService.CORE/"]
COPY ["MainService.INFRASTRUCTURE/MainService.INFRASTRUCTURE.csproj", "MainService.INFRASTRUCTURE/"]
RUN dotnet restore "./MainService.API/MainService.API.csproj"
COPY . .
WORKDIR "/src/MainService.API"
RUN dotnet build "./MainService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MainService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainService.API.dll"]
