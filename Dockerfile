
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

ENTRYPOINT ["dotnet", "idvault-server.dll"]


