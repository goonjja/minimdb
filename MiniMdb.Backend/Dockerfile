#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

# Uncomment these lines when debugging in VS
#RUN apt-get update -yq
#RUN apt-get install curl gnupg -yq
#RUN curl -sL https://deb.nodesource.com/setup_12.x | bash
#RUN apt-get install -yq nodejs build-essential

WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MiniMdb.Backend/MiniMdb.Backend.csproj", "MiniMdb.Backend/"]
COPY ["MiniMdb.Auth/MiniMdb.Auth.csproj", "MiniMdb.Auth/"]
RUN dotnet restore "MiniMdb.Backend/MiniMdb.Backend.csproj"
COPY . .

WORKDIR "/src/MiniMdb.Backend"
RUN dotnet build "MiniMdb.Backend.csproj" -c Release -o /app/build

FROM build AS publish

RUN apt-get update -yq
RUN apt-get install curl gnupg -yq
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash
RUN apt-get install -yq nodejs build-essential

RUN dotnet publish "MiniMdb.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniMdb.Backend.dll"]