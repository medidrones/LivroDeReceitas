FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY src ./
WORKDIR /app/Backend/LivroDeReceitas.Api

RUN dotnet restore

# Build
RUN dotnet publish -c Release -o ../../out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "LivroDeReceitas.Api.dll"]
