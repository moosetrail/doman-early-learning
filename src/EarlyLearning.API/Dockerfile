#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/EarlyLearning.API/EarlyLearning.API.csproj", "src/EarlyLearning.API/"]
COPY ["src/EarlyLearning.People/EarlyLearning.People.csproj", "src/EarlyLearning.People/"]
COPY ["src/EarlyLearning.Core/EarlyLearning.Core.csproj", "src/EarlyLearning.Core/"]
COPY ["src/EarlyLearning.RavenDb/EarlyLearning.RavenDb.csproj", "src/EarlyLearning.RavenDb/"]
COPY ["src/EarlyLearning.RavenDb.Setup/EarlyLearning.RavenDb.Setup.csproj", "src/EarlyLearning.RavenDb.Setup/"]
COPY ["src/EarlyLearning.ReadingProgram/EarlyLearning.ReadingProgram.csproj", "src/EarlyLearning.ReadingProgram/"]
COPY ["src/EarlyLearning.API.DependencyInjection/EarlyLearning.API.DependencyInjection.csproj", "src/EarlyLearning.API.DependencyInjection/"]
RUN dotnet restore "src/EarlyLearning.API/EarlyLearning.API.csproj"
COPY . .
WORKDIR "/src/src/EarlyLearning.API"
RUN dotnet build "EarlyLearning.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EarlyLearning.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EarlyLearning.API.dll"]