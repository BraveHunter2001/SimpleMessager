FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 3000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SimpleMessager/SimpleMessagerApi.csproj", "SimpleMessager/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Services/Services.csproj", "Services/"]
RUN dotnet restore "SimpleMessager/SimpleMessagerApi.csproj"
COPY . .

WORKDIR  /src/SimpleMessager
RUN dotnet build "SimpleMessagerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleMessagerApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleMessagerApi.dll"]