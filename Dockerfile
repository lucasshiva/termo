# -----------------------------
# 1. Build frontend (Vue + Vite + Bun)
# -----------------------------
FROM oven/bun:1 AS frontend-build
WORKDIR /ui

# Without Node, `bun run build` will fail due to `vue-tsc`
RUN apt-get update && apt-get install -y nodejs

COPY termo-ui/package.json termo-ui/bun.lock ./
RUN bun install

COPY termo-ui .
RUN bun run build


# -----------------------------
# 2. Build backend (ASP.NET)
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend-build
WORKDIR /api

COPY termo-api/Termo.Api/*.csproj ./
RUN dotnet restore

COPY termo-api .
RUN dotnet publish Termo.Api -c Release -o /out


# -----------------------------
# 3. Runtime image
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine

# Install nginx
RUN apk add --no-cache nginx bash

WORKDIR /app

# Copy backend
COPY --from=backend-build /out ./api

# Copy frontend
COPY --from=frontend-build /ui/dist /usr/share/nginx/html

# Copy nginx config
COPY nginx.conf /etc/nginx/http.d/default.conf

# Copy startup script
COPY start.sh /start.sh
RUN chmod +x /start.sh

EXPOSE 80

ENTRYPOINT ["/start.sh"]
