# ProjetoAcademia — API

Backend do Projeto Academia: API que guarda as fichas de treino, as sessões executadas série a
série, o cardio e o histórico de peso de cada usuário. Cada usuário só enxerga os próprios dados.

Arquitetura, modelo de dados e convenções estão no
[`Planejamento.md`](https://github.com/Zanatta97/ProjetoAcademia/blob/main/Planejamento.md) do repo
pai.

## Stack

- .NET 10 (LTS), ASP.NET Core com controllers
- EF Core + SQL Server (Azure SQL Database em produção), tabelas e colunas em snake_case
- ASP.NET Core Identity com bearer token e refresh
- AutoMapper e FluentValidation
- OpenAPI nativo + Scalar
- xUnit, `WebApplicationFactory` e Testcontainers nos testes

## Estrutura

Clean Architecture com um projeto por camada: `Domain` → `Application` → `Infrastructure` → `Api`.

## Como rodar

> A preencher conforme o projeto crescer (pré-requisitos, connection string, migrations, porta).
