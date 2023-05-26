FROM mcr.microsoft.com/dotnet/aspnet:3.1.28-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1.422-bullseye AS build
WORKDIR /src
COPY ["TStore.SaleApi/TStore.SaleApi.csproj", "TStore.SaleApi/"]
RUN dotnet restore "TStore.SaleApi/TStore.SaleApi.csproj"
COPY . .
WORKDIR "/src/TStore.SaleApi"
RUN dotnet build "TStore.SaleApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TStore.SaleApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD sleep 30s; dotnet TStore.SaleApi.dll