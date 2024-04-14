# Use the official Microsoft ASP.NET Core runtime image
# Adjust the version as necessary for .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AnalystPortal.API.csproj", "./"]
RUN dotnet restore "AnalystPortal.API.csproj"
COPY . .
RUN dotnet build "AnalystPortal.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AnalystPortal.API.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/Images   # Ensure the Images directory exists
ENTRYPOINT ["dotnet", "AnalystPortal.API.dll"]
