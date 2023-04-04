FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/OpenAi/OpenAi.API/OpenAi.API.csproj", "src/OpenAi/OpenAi.API/"]
RUN dotnet restore "src/OpenAi/OpenAi.API/OpenAi.API.csproj"
COPY . .
WORKDIR "/src/src/OpenAi/OpenAi.API"
RUN dotnet build "OpenAi.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenAi.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenAi.API.dll"]
