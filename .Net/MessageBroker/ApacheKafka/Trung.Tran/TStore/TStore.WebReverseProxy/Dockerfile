FROM mcr.microsoft.com/dotnet/aspnet:3.1.28-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1.422-bullseye AS build
WORKDIR /src
COPY ["TStore.WebReverseProxy/TStore.WebReverseProxy.csproj", "TStore.WebReverseProxy/"]
RUN dotnet restore "TStore.WebReverseProxy/TStore.WebReverseProxy.csproj"
COPY . .
WORKDIR "/src/TStore.WebReverseProxy"
RUN dotnet build "TStore.WebReverseProxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TStore.WebReverseProxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TStore.WebReverseProxy.dll"]