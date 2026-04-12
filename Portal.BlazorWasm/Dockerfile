# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copia os arquivos de projeto individualmente
# O Docker espera que esses arquivos estejam em: contexto/Pasta/Arquivo
COPY ["Portal.BlazorWasm/Portal.BlazorWasm.csproj", "Portal.BlazorWasm/"]
COPY ["Portal.Application/Portal.Application.csproj", "Portal.Application/"]

# 2. Restaura as dependências
# Se falhar aqui, o nome da pasta ou do arquivo está diferente (ex: .Application vs .application)
RUN dotnet restore "Portal.BlazorWasm/Portal.BlazorWasm.csproj"

# 3. Copia o restante dos arquivos da solução para o container
COPY . .

# 4. Entra na pasta do projeto e publica
WORKDIR "/src/Portal.BlazorWasm"
RUN dotnet publish "Portal.BlazorWasm.csproj" -c Release -o /app/publish

# Estágio de Runtime (Nginx)
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]