# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY AlmaIntegrationTools.AccountSync/AlmaIntegrationTools.AccountSync/AlmaIntegrationTools.AccountSync.csproj AlmaIntegrationTools.AccountSync/AlmaIntegrationTools.AccountSync/AlmaIntegrationTools.AccountSync.csproj
COPY AlmaIntegrationTools.Bursar/AlmaIntegrationTools.Bursar/AlmaIntegrationTools.Bursar.csproj AlmaIntegrationTools.Bursar/AlmaIntegrationTools.Bursar/AlmaIntegrationTools.Bursar.csproj
COPY AlmaIntegrationTools.Finance/AlmaIntegrationTools.Finance/AlmaIntegrationTools.Finance.csproj AlmaIntegrationTools.Finance/AlmaIntegrationTools.Finance/AlmaIntegrationTools.Finance.csproj
COPY AlmaIntegrationTools/AlmaIntegrationTools/AlmaIntegrationTools.csproj AlmaIntegrationTools/AlmaIntegrationTools/AlmaIntegrationTools.csproj

# restore projcts
RUN dotnet restore AlmaIntegrationTools/AlmaIntegrationTools/AlmaIntegrationTools.csproj
RUN dotnet restore AlmaIntegrationTools.AccountSync/AlmaIntegrationTools.AccountSync/AlmaIntegrationTools.AccountSync.csproj
RUN dotnet restore AlmaIntegrationTools.Bursar/AlmaIntegrationTools.Bursar/AlmaIntegrationTools.Bursar.csproj
RUN dotnet restore AlmaIntegrationTools.Finance/AlmaIntegrationTools.Finance/AlmaIntegrationTools.Finance.csproj

# copy and build app and libraries
COPY AlmaIntegrationTools.AccountSync AlmaIntegrationTools.AccountSync
COPY AlmaIntegrationTools.Bursar AlmaIntegrationTools.Bursar
COPY AlmaIntegrationTools.Finance AlmaIntegrationTools.Finance
COPY AlmaIntegrationTools AlmaIntegrationTools

# restore account sync
WORKDIR /source/AlmaIntegrationTools.AccountSync
RUN dotnet build -c release --no-restore

# restore account bursar
WORKDIR /source/AlmaIntegrationTools.Bursar
RUN dotnet build -c release --no-restore

# restore account fianance
WORKDIR /source/AlmaIntegrationTools.Finance
RUN dotnet build -c release --no-restore


# build publish image
FROM build AS publish

# account sync
WORKDIR /source/AlmaIntegrationTools.AccountSync
RUN dotnet publish -c release --no-build -o /app

# bursar
WORKDIR /source/AlmaIntegrationTools.Bursar
RUN dotnet publish -c release --no-build -o /app

# finance
WORKDIR /source/AlmaIntegrationTools.Finance
RUN dotnet publish -c release --no-build -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=publish /app .

# configure environment
RUN mkdir /AccountSync
COPY private/ssh-keys ssh-keys

# default
CMD ["dotnet", "AlmaIntegrationTools.AccountSync.dll"]
