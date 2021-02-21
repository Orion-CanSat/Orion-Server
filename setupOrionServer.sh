#!/bin/bash

SQL_VERSION=8.0.1
SQL_CONTAINER_NAME=orionmysqlserver

SERVER_CONTAINER_IMAGE=orionserverimage
SERVER_CONTAINER_NAME=orionserver

printf "Please type your MySQL Password [The password will be hidden]: "
read -s SQLPASSWORD

docker pull "mysql:$SQL_VERSION"
docker container stop "$SQL_CONTAINER_NAME"
docker container rm "$SQL_CONTAINER_NAME"
docker run --name "$SQL_CONTAINER_NAME" -d -e "MYSQL_ROOT_PASSWORD=$SQLPASSWORD" "mysql:$SQL_VERSION"
SQL_IP=$(docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' "$SQL_CONTAINER_NAME")

docker build -t "$SERVER_CONTAINER_IMAGE" --build-arg SQL_IP="$SQL_IP" --build-arg SQLPASSWORD="$SQLPASSWORD" .
docker container stop "$SERVER_CONTAINER_NAME"
docker container rm "$SERVER_CONTAINER_NAME"
docker run --name "$SERVER_CONTAINER_NAME" -d --link "$SQL_CONTAINER_NAME:db" -p 5000:80 "$SERVER_CONTAINER_IMAGE"
