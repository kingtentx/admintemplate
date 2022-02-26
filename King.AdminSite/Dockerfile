#--------源码编译打包
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
#WORKDIR /src
#COPY ["King.AdminSite/King.AdminSite.csproj", "King.AdminSite/"]
#RUN dotnet restore "King.AdminSite/King.AdminSite.csproj"
#COPY . .
#WORKDIR "/src/King.AdminSite"
#RUN dotnet build "King.AdminSite.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "King.AdminSite.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "King.AdminSite.dll"]
#----------

#-----编译文件
#FROM lwy19880704/dotnet-aspnet-gdi:3.1 AS base
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
FROM 172.16.100.181:5000/netcore3:v1
COPY . /app
WORKDIR /app

ENV ASPNETCORE_URLS http://*:5000
ENV TZ Asia/Shanghai
EXPOSE 5000

ENTRYPOINT ["dotnet", "King.AdminSite.dll"]

#----------
