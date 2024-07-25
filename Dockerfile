FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV WEATHERAPI_API_KEY="4888f52ea03a4d3299c114525242407"
ENV OPENWEATHER_API_KEY="7bd3c30013b8631827819ef1a206877d"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["WeatherForecast/WeatherForecast.API.csproj", "WeatherForecast/"]
COPY ["WeatherForecast.BL/WeatherForecast.BL.csproj", "WeatherForecast.BL/"]
RUN dotnet restore "WeatherForecast/WeatherForecast.API.csproj"

COPY . .
WORKDIR "/src/WeatherForecast"
RUN dotnet build "WeatherForecast.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherForecast.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherForecast.API.dll"]