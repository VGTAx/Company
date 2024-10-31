FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20-amd64 AS build
WORKDIR /src

COPY . .
# Restore dependencies. 
# --runtime flag points at platform which using for restore
RUN dotnet restore "Company.csproj" --runtime linux-musl-x64

RUN dotnet publish "Company.csproj" \
    # build configuration ex. Debug or Release
    -c Release \ 
    # path for the output directory.
    -o /app/publish \
    # doesn't execute restore when running command
    --no-restore \
    # platform which using for publish
    --runtime linux-musl-x64
    
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.20-amd64

WORKDIR /app
COPY --from=build /app/publish /app
ENTRYPOINT [ "./Company" ]