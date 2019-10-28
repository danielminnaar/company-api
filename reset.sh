#! /bin/bash
docker-compose down
docker volume rm company-api_db_volume
docker build -t company-api .
docker-compose up
