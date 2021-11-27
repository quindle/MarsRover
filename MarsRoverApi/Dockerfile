#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MarsRoverApi/MarsRoverApi.csproj", "MarsRoverApi/"]
RUN dotnet restore "MarsRoverApi/MarsRoverApi.csproj"
COPY . .
WORKDIR "/src/MarsRoverApi"
RUN dotnet build "MarsRoverApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarsRoverApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarsRoverApi.dll"]