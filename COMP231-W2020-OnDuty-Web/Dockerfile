#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
COPY . .

CMD ASPNETCORE_URLS=http://*$PORT dotnet COMP231_W2020_OnDuty_Web.dll