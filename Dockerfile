FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
ENV PATH="$PATH:/root/.dotnet/tools"
WORKDIR /app

# Copy everything and build
COPY . ./
RUN cd "Orion Server" && libman restore
RUN dotnet publish -c Release -r linux-x64 --self-contained true -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-amd64
WORKDIR /app
COPY --from=build-env /app/out .
RUN echo "{\"DatabaseIP\": \"$SQL_IP\",\"DatabaseID\": \"root\",\"DatabasePassword\": \"$SQLPASSWORD\",\"DatabaseName\": \"Orion_Database\",\"DatabaseType\": \"MySQL\"}" > /app/wwwdata/database.json

ENTRYPOINT ["./OrionServer"]
