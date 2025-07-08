# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PetTrack/PetTrack.csproj", "PetTrack/"]
COPY ["PetTrack.Contract.Repositories/PetTrack.Contract.Repositories.csproj", "PetTrack.Contract.Repositories/"]
COPY ["PetTrack.Core/PetTrack.Core.csproj", "PetTrack.Core/"]
COPY ["PetTrack.Contract.Services/PetTrack.Contract.Services.csproj", "PetTrack.Contract.Services/"]
COPY ["PetTrack.Entity/PetTrack.Entity.csproj", "PetTrack.Entity/"]
COPY ["PetTrack.ModelViews/PetTrack.ModelViews.csproj", "PetTrack.ModelViews/"]
COPY ["PetTrack.Repositories/PetTrack.Repositories.csproj", "PetTrack.Repositories/"]
COPY ["PetTrack.Services/PetTrack.Services.csproj", "PetTrack.Services/"]
RUN dotnet restore "./PetTrack/PetTrack.csproj"
COPY . .
WORKDIR "/src/PetTrack"
RUN dotnet build "./PetTrack.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetTrack.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetTrack.dll"]