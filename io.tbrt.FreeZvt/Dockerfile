FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["io.tbrt.FreeZvt/io.tbrt.FreeZvt.csproj", "io.tbrt.FreeZvt/"]
RUN dotnet restore "io.tbrt.FreeZvt/io.tbrt.FreeZvt.csproj"
COPY . .
WORKDIR "/src/io.tbrt.FreeZvt"
RUN dotnet build "io.tbrt.FreeZvt.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "io.tbrt.FreeZvt.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "io.tbrt.FreeZvt.dll"]
