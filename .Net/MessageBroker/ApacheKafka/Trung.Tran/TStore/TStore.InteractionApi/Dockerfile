FROM mcr.microsoft.com/dotnet/aspnet:3.1.28-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1.422-bullseye AS build
WORKDIR /src
COPY ["TStore.InteractionApi/TStore.InteractionApi.csproj", "TStore.InteractionApi/"]
RUN dotnet restore "TStore.InteractionApi/TStore.InteractionApi.csproj"
COPY . .
WORKDIR "/src/TStore.InteractionApi"
RUN dotnet build "TStore.InteractionApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TStore.InteractionApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD sleep 30s; dotnet TStore.InteractionApi.dll