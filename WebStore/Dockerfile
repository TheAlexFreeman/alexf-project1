FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

WORKDIR /app

COPY *.sln ./
COPY WebStore.App/*.csproj WebStore.App/
COPY WebStore.BLL/*.csproj WebStore.BLL/
COPY WebStore.Data/*.csproj WebStore.Data/
COPY WebStore.Tests/*.csproj WebStore.Tests/

RUN dotnet restore

COPY . ./

# ARG CONFIG=Release
RUN dotnet build

RUN dotnet test

RUN dotnet publish WebStore.App --no-build --output out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app

COPY --from=build /app/WebStore.App/out ./

ENTRYPOINT [ "dotnet", "WebStore.App.dll" ]