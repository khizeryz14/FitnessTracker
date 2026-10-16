# ---- build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy only the project file first so the restore layer is cached
COPY FitnessTracker/FitnessTracker.csproj FitnessTracker/
RUN dotnet restore FitnessTracker/FitnessTracker.csproj

# now copy the source and publish
COPY FitnessTracker/ FitnessTracker/
RUN dotnet publish FitnessTracker/FitnessTracker.csproj \
    -c Release -o /app/publish --no-restore -p:UseAppHost=false

# ---- runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "FitnessTracker.dll"]