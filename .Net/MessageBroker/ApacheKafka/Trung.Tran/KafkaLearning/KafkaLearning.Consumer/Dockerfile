FROM mcr.microsoft.com/dotnet/runtime:3.1.28-bullseye-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1.422-bullseye AS build
WORKDIR /src
COPY ["KafkaLearning.Consumer/KafkaLearning.Consumer.csproj", "KafkaLearning.Consumer/"]
RUN dotnet restore "KafkaLearning.Consumer/KafkaLearning.Consumer.csproj"
COPY . .
WORKDIR "/src/KafkaLearning.Consumer"
RUN dotnet build "KafkaLearning.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KafkaLearning.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD sleep 7s; dotnet KafkaLearning.Consumer.dll
