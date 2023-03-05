FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KvolikDubBackend.csproj", ""]
RUN dotnet restore "KvolikDubBackend.csproj"
COPY . .
WORKDIR "/src/KvolikDubBackend"
RUN dotnet build "../KvolikDubBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "../KvolikDubBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KvolikDubBackend.dll"]