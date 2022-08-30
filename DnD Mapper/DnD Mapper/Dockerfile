#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 1001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DnD Mapper/DnD Mapper.csproj", "DnD Mapper/"]
RUN dotnet restore "DnD Mapper/DnD Mapper.csproj"
COPY . .
WORKDIR "/src/DnD Mapper"
RUN dotnet build "DnD Mapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DnD Mapper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DnD Mapper.dll"]