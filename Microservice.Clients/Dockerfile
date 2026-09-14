FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["Microservice.Clients/Microservice.Clients.csproj", "Microservice.Clients/"]
RUN dotnet restore "Microservice.Clients/Microservice.Clients.csproj"

# copy everything else and publish
COPY . .
WORKDIR "/src/Microservice.Clients"
RUN dotnet publish "Microservice.Clients.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Microservice.Clients.dll"]
