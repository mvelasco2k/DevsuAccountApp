FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["Microservice.Accounts/Microservice.Accounts.csproj", "Microservice.Accounts/"]
RUN dotnet restore "Microservice.Accounts/Microservice.Accounts.csproj"

# copy everything else and publish
COPY . .
WORKDIR "/src/Microservice.Accounts"
RUN dotnet publish "Microservice.Accounts.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Microservice.Accounts.dll"]
