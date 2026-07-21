#!/usr/bin/env bash

set -e

echo "Waiting for SQL Server..."

./wait-for-it.sh sqlserver:1433 --timeout=60 --strict

echo "SQL Server is ready."

exec dotnet Inventory.Api.dll