# ProjetoAcademia — Backend

API .NET 10 do Projeto Academia. Arquitetura, stack, modelo de dados e convenções estão no
`Planejamento.md` do repo pai (`../Planejamento.md`). O `CLAUDE.md` do repo pai também vale aqui.

## Modo mentoria — leia antes de qualquer coisa

**Este backend é escrito pelo usuário, à mão.** A regra abaixo tem precedência sobre o fluxo do
CLAUDE.md global:

- **Todo pedido sobre este repositório é uma dúvida.** Responder no chat, sem plano, sem issue, sem
  branch e sem agente de implementação.
- **Nenhum arquivo deste repositório é criado ou alterado sem pedido explícito** ("altere",
  "crie no projeto", "implemente você"). "Como faço?", "me mostra", "cria um exemplo" **não** são
  pedidos explícitos.
- **Resposta padrão: exemplo em texto no chat**, que o usuário replica. Explicar o porquê das
  decisões, não só o código.
- Comandos que alteram o projeto (`dotnet new`, `dotnet add`, `dotnet ef migrations add`,
  `database update`, `git` com escrita) vão no chat para o usuário rodar. Leitura (`dotnet build`,
  `dotnet test`, `git status`) pode rodar quando ajudar a responder.
- Com pedido explícito de alteração, volta o fluxo normal (plano → aprovação → execução).

## Resumo do que o código deve seguir

- Clean Architecture com um projeto por camada: `Domain` → `Application` → `Infrastructure` → `Api`.
- `Domain` sem referência a nada; `Application` só `Domain` + AutoMapper + FluentValidation;
  `Infrastructure` com EF Core / Identity; `Api` usa `Infrastructure` só no `Program.cs`.
- Um caso de uso por classe (`...UseCase`), sem MediatR. Controllers chamam casos de uso.
- `UserId` vem sempre de `ICurrentUser`, nunca do corpo ou da rota.
- Identificadores em inglês; mensagens, comentários e documentação em português.
- Banco em snake_case, tabelas no plural.

Base de estudo (não oficial): `../docs/backend-base.md`.
