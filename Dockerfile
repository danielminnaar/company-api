FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app
COPY api-src/*.csproj ./
RUN dotnet restore api-src.csproj
COPY api-src/. ./
RUN dotnet publish api-src.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "api-src.dll"]