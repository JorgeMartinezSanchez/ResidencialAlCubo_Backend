# Etapa 1: Compilación usando el SDK oficial de .NET 10
FROM ://microsoft.com AS build-env
WORKDIR /app

# Copiar archivos del proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código y compilar la aplicación
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: Crear la imagen ligera de ejecución
FROM ://microsoft.com
WORKDIR /app
COPY --from=build-env /app/out .

# Configurar el puerto dinámico para Railway
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "rec-be.dll"]
