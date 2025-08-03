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
COPY ["MyFinanceService.API/MyFinanceService.API.csproj", "MyFinanceService.API/"]
COPY ["MyFinanceService.APPLICATION/MyFinanceService.APPLICATION.csproj", "MyFinanceService.APPLICATION/"]
COPY ["MyFinanceService.CORE/MyFinanceService.CORE.csproj", "MyFinanceService.CORE/"]
COPY ["MyFinanceService.INFRASTRUCTURE/MyFinanceService.INFRASTRUCTURE.csproj", "MyFinanceService.INFRASTRUCTURE/"]
RUN dotnet restore "./MyFinanceService.API/MyFinanceService.API.csproj"
COPY . .
WORKDIR "/src/MyFinanceService.API"
RUN dotnet build "./MyFinanceService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyFinanceService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFinanceService.API.dll"] 