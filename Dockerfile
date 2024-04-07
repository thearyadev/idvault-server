
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src
COPY . .
RUN dotnet restore "./idvault-server.csproj"
RUN dotnet build "./idvault-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./idvault-server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DB_CONNECTION_STRING="Server=192.168.1.160;port=5432;Database=postgres;User id=postgres;Password=idvault_server;"

ENTRYPOINT ["dotnet", "idvault-server.dll"]


