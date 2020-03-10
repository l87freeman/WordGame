#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8086

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WordGame.Dictionary/WordGame.Dictionary.csproj", "WordGame.Dictionary/"]
COPY ["WordGame.Common/WordGame.Common.csproj", "WordGame.Common/"]
RUN dotnet restore "WordGame.Dictionary/WordGame.Dictionary.csproj"
COPY . .
WORKDIR "/src/WordGame.Dictionary"
RUN dotnet build "WordGame.Dictionary.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WordGame.Dictionary.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WordGame.Dictionary.dll"]