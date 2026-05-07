# Çalışma zamanı imajı (.NET 9)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Derleme imajı (.NET 9 SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Proje dosyasını kopyalayıp restore yapıyoruz
COPY ["src/Notification.Api/Notification.Api.csproj", "src/Notification.Api/"]
RUN dotnet restore "src/Notification.Api/Notification.Api.csproj"

# Tüm dosyaları kopyalayıp derliyoruz
COPY . .
WORKDIR "/src/src/Notification.Api"
RUN dotnet build "Notification.Api.csproj" -c Release -o /app/build

# Yayınlama aşaması
FROM build AS publish
RUN dotnet publish "Notification.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final imajı
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.Api.dll"]