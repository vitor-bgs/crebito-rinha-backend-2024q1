FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.18 AS build-env
WORKDIR /App

COPY ./Crebito.sln ./Crebito.sln
COPY ./src/Crebito.Api/Crebito.Api.csproj ./src/Crebito.Api/Crebito.Api.csproj
COPY ./src/Crebito.Common/Crebito.Common.csproj ./src/Crebito.Common/Crebito.Common.csproj
COPY ./src/Crebito.Domain/Crebito.Domain.csproj ./src/Crebito.Domain/Crebito.Domain.csproj
COPY ./src/Crebito.Infra.DataAccess.WithDapper/Crebito.Infra.DataAccess.WithDapper.csproj ./src/Crebito.Infra.DataAccess.WithDapper/Crebito.Infra.DataAccess.WithDapper.csproj

# Restore as distinct layers
RUN dotnet restore

# Copy everything
COPY . ./

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.18
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Crebito.Api.dll"]
