FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ComedyMcpServer.csproj", "./"]
RUN dotnet restore "ComedyMcpServer.csproj"
COPY . .
RUN dotnet build "ComedyMcpServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ComedyMcpServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ComedyMcpServer.dll"] 