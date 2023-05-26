FROM mcr.microsoft.com/dotnet/aspnet:3.1.28-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1.422-bullseye AS build
WORKDIR /src
COPY ["TStore.SystemApi/TStore.SystemApi.csproj", "TStore.SystemApi/"]
RUN dotnet restore "TStore.SystemApi/TStore.SystemApi.csproj"
COPY . .
WORKDIR "/src/TStore.SystemApi"
RUN dotnet build "TStore.SystemApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TStore.SystemApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD sleep 25s; dotnet TStore.SystemApi.dll