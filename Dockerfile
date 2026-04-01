FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de proyecto y restaurar dependencias
COPY *.csproj .
RUN dotnet restore

# Copiar el resto de archivos y publicar
COPY . .
RUN dotnet publish -c Release -o out

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Configurar el puerto
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Comando de inicio
ENTRYPOINT ["dotnet", "MedicamentosAPI.dll"]