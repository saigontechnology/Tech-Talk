FROM mcr.microsoft.com/dotnet/runtime:3.1.28-bullseye-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1.422-bullseye AS build
WORKDIR /src
COPY ["Consumers/TStore.Consumers.InteractionAnalysis/TStore.Consumers.InteractionAnalysis.csproj", "Consumers/TStore.Consumers.InteractionAnalysis/"]
RUN dotnet restore "Consumers/TStore.Consumers.InteractionAnalysis/TStore.Consumers.InteractionAnalysis.csproj"
COPY . .
WORKDIR "/src/Consumers/TStore.Consumers.InteractionAnalysis"
RUN dotnet build "TStore.Consumers.InteractionAnalysis.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TStore.Consumers.InteractionAnalysis.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD sleep 30s; dotnet TStore.Consumers.InteractionAnalysis.dll
