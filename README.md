# Portal - Sistema de Controle de Comissões de Vendedores

## 📌 Sobre o Projeto

Este projeto foi desenvolvido como solução para um desafio técnico, com o objetivo de construir um sistema completo para **gestão de comissões de vendedores**, contemplando:

* Cadastro de vendedores
* Controle de invoices (faturas)
* Cálculo automático de comissões
* Dashboard gerencial com indicadores
* Testes unitários das regras de negócio

Como diferenciais foram adicionados:
* Documentação da API (Swagger)
* Docker
* Migrations

A aplicação foi construída utilizando **.NET (C#)** com foco em **boas práticas, arquitetura limpa e escalabilidade**.

---

## 🧠 Arquitetura

O projeto segue os princípios da Clean Architecture para garantir desacoplamento:

```
Portal.Domain: Entidades de negócio (Vendedor, Invoice) e regras fundamentais.

Portal.Application: DTOs e Serviços de Aplicação.

Portal.Infra.Data.Repository: Implementação do DbContext e Repositórios.

Portal.Web: Interface MVC, Controllers e configuração de Injeção de Dependência.
```
---
## ⚙️ Tecnologias Utilizadas

* .NET 9 (C#)
* ASP.NET Web API
* Entity Framework Core
* Banco de Dados: SQL Server 2022
* Swagger (documentação da API)
* xUnit (testes unitários)
* FluentAssertions (assertivas mais legíveis)
* Arquitetura: Clean Architecture (Domain, Application, Infra, Web)
* Containerização: Docker & Docker Compose

---

## ▶️ Como Executar o Projeto
Você não precisa ter o .NET ou SQL Server instalados localmente, apenas o Docker Desktop.

### 1. Clonar o repositório:

```bash
git clone https://github.com/Geanhm/Portal.git
```

---

### 2. Suba os containers:

```bash
docker-compose up -d --build
```

### 3. Acessar o sistema:

```
Dashboard: http://localhost:5000

Swagger (API): http://localhost:5000/swagger
```
---
### Funcionalidades Principais

* Dashboard de Vendas: Visão geral de invoices e comissões.
* Gestão de Vendedores: Cadastro completo com validação e limpeza automática de CPF.
* Gestão de Invoices: Cadastro completo com cálculo de comissão
* Resiliência: Healthchecks configurados para garantir que a API só inicie após o banco de dados estar pronto.


## 🔄 Fluxo Principal

1. Criar um vendedor
2. Criar uma invoice vinculada ao vendedor
3. O sistema calcula automaticamente a comissão
4. Consultar os dados no dashboard

--- 
## 🧪 Testes Unitários

O projeto possui cobertura de testes para:

* Validação de CPF e Email
* Regras de criação de Vendedor
* Validação de Invoice
* Cálculo de comissão
* Regras de aprovação e cancelamento

Para executar:

```bash
dotnet test
```

---

## 🚀 Possíveis Melhorias

* Autenticação e autorização (JWT)
* Implementação de paginação nas consultas
* Exportação de relatórios (PDF/Excel)
* Logs e auditoria
* Mensageria (Rabbit): Para atender altas demandas

---

## 👨‍💻 Autor

Desenvolvido por **Gean H. Marzarotto**
[[LinkedIn](https://www.linkedin.com/in/geanhomem/)] • [[GitHub](https://github.com/Geanhm/)]

---

## Notas de Desenvolvimento

* Ambiente configurado via Docker Compose: Development
* Dica para Desenvolvimento Local:
Caso deseje debugar a API fora do Docker (via Visual Studio) mantendo o banco no container, certifique-se de que a porta 1433 está mapeada e ajuste o Server na ConnectionString para localhost:
```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PortalDb;User Id=sa;Password=narwal@2026;TrustServerCertificate=True;"
}	  

```
Links para teste local:
```
Dashboard: https://localhost:44352
Swagger (API): http://localhost:44352/swagger/index.html
```
---

⭐ Obrigado pela oportunidade!
