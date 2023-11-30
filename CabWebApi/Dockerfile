#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CabWebApi/CabWebApi.csproj", "CabWebApi/"]
COPY ["CabWebApi.Domain.Core/CabWebApi.Domain.Core.csproj", "CabWebApi.Domain.Core/"]
COPY ["CabWebApi.Domain.Interfaces/CabWebApi.Domain.Interfaces.csproj", "CabWebApi.Domain.Interfaces/"]
COPY ["CabWebApi.Infrastructure.Business/CabWebApi.Infrastructure.Business.csproj", "CabWebApi.Infrastructure.Business/"]
COPY ["CabWebApi.Services.Interfaces/CabWebApi.Services.Interfaces.csproj", "CabWebApi.Services.Interfaces/"]
COPY ["CabWebApi.Infrastructure.Data/CabWebApi.Infrastructure.Data.csproj", "CabWebApi.Infrastructure.Data/"]
RUN dotnet restore "CabWebApi/CabWebApi.csproj"
COPY . .
WORKDIR "/src/CabWebApi"
RUN dotnet build "CabWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CabWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CabWebApi.dll"]