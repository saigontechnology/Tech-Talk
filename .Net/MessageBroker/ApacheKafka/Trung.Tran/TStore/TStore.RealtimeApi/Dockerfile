FROM mcr.microsoft.com/dotnet/aspnet:3.1.28-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1.422-bullseye AS build
WORKDIR /src
COPY ["TStore.RealtimeApi/TStore.RealtimeApi.csproj", "TStore.RealtimeApi/"]
RUN dotnet restore "TStore.RealtimeApi/TStore.RealtimeApi.csproj"
COPY . .
WORKDIR "/src/TStore.RealtimeApi"
RUN dotnet build "TStore.RealtimeApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TStore.RealtimeApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TStore.RealtimeApi.dll"]