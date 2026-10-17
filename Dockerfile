# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the project file first and restore packages (cached layer)
COPY TaskManager.csproj .
RUN dotnet restore

# Copy the rest of the source code and publish
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Directory for the SQLite database (mounted as a volume), owned by the non-root user
RUN mkdir /app/data && chown app:app /app/data
USER app

COPY --from=build /app/publish .

ENV ConnectionStrings__Default="Data Source=/app/data/tasks.db"
EXPOSE 8080

ENTRYPOINT ["dotnet", "TaskManager.dll"]
