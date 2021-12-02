#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

# install System.Drawing native dependencies  
RUN ln -s /lib/x86_64-linux-gnu/libdl-2.28.so /lib/x86_64-linux-gnu/libdl.so  
RUN apt-get update && apt-get install -y --allow-unauthenticated libgdiplus libc6-dev libx11-dev  
RUN ln -s libgdiplus.so gdiplus.dll 

WORKDIR /app
COPY cert/cert.pfx /app/cert/cert.pfx
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# install System.Drawing native dependencies  --> Uncomment saat publish
RUN ln -s /lib/x86_64-linux-gnu/libdl-2.28.so /lib/x86_64-linux-gnu/libdl.so  
RUN apt-get update && apt-get install -y --allow-unauthenticated libgdiplus libc6-dev libx11-dev  
RUN ln -s libgdiplus.so gdiplus.dll 

WORKDIR /src
COPY ["simpat1k.csproj", "."]
RUN dotnet restore "simpat1k.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "simpat1k.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "simpat1k.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=https://+:443;http://+:80
ENTRYPOINT ["dotnet", "simpat1k.dll"]
