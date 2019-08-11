FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

WORKDIR /app

COPY ./WebStore.App.csproj ./
COPY WebStore/WebStore.BLL/WebStore.BLL.csproj ./
COPY ../WebStore.Data/WebStore.Data.csproj ./
COPY ../WebStore.Tests/WebStore.Tests.csproj ./

RUN dotnet restore

COPY . ./

RUN dotnet build --no-restore

RUN dotnet publish --no-build -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app

COPY --from=build /app/out ./

CMD ["dotnet", "WebStore.App.dll"]
