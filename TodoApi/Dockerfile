# שלב 1: Image בסיסי להרצה
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80 443
ENV ToDoDB=${ToDoDB}

# שלב 2: Image לבנייה
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TodoApi.csproj", "./"]
RUN dotnet restore "TodoApi.csproj"
COPY . .
RUN dotnet build "TodoApi.csproj" -c Release -o /app/build

# שלב 3: פרסום
FROM build AS publish
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish

# שלב 4: הרצה
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]