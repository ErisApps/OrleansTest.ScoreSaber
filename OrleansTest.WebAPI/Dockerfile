FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrleansTest.WebAPI/OrleansTest.WebAPI.csproj", "OrleansTest.WebAPI/"]
RUN dotnet restore "OrleansTest.WebAPI/OrleansTest.WebAPI.csproj"
COPY . .
WORKDIR "/src/OrleansTest.WebAPI"
RUN dotnet build "OrleansTest.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrleansTest.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrleansTest.WebAPI.dll"]
