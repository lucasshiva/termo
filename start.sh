#!/bin/sh

# Start backend
dotnet /app/api/Termo.Api.dll &

# Start nginx (foreground)
nginx -g "daemon off;"
