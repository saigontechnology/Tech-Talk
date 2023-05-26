FROM mcr.microsoft.com/mssql/server:2019-CU18-ubuntu-20.04 AS base
EXPOSE 1433

FROM mcr.microsoft.com/dotnet/sdk:3.1.422-bullseye AS build
WORKDIR /src
COPY ["SimulateMemoryUsageTool/SimulateMemoryUsageTool.csproj", "SimulateMemoryUsageTool/"]
RUN dotnet restore "SimulateMemoryUsageTool/SimulateMemoryUsageTool.csproj"
COPY . .
WORKDIR "/src/SimulateMemoryUsageTool"
RUN dotnet build "SimulateMemoryUsageTool.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimulateMemoryUsageTool.csproj" --self-contained true -r linux-x64 -c Release -o /app/publish

FROM base AS final
WORKDIR /mem-tool
COPY --from=publish /app/publish .