FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TBRT.FreeZVT.Host/TBRT.FreeZVT.Host.csproj", "TBRT.FreeZVT.Host/"]
RUN dotnet restore "TBRT.FreeZVT.Host/TBRT.FreeZVT.Host.csproj"
COPY . .
WORKDIR "/src/TBRT.FreeZVT.Host"
RUN dotnet build "TBRT.FreeZVT.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TBRT.FreeZVT.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TBRT.FreeZVT.Host.dll"]
