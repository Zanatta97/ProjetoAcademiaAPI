using Microsoft.AspNetCore.Identity;
using ProjetoAcademiaAPI.Api.Configuration;
using ProjetoAcademiaAPI.Infra.DI;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Continua usando o Ilooger e o Serilog apenas assume a saída dos dados
builder.Services.AddSerilog((services, cfg) => cfg
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services));

//Adiciona o banco de dados da aplicação com SQL Server, com snake_case e configuração de retry em caso de falha de conexão.
builder.Services.AddInfra(builder.Configuration);

//Adiciona todas as configurações da API, incluindo o Swagger, CORS, Controllers, etc.
builder.Services.AddApiConfigurations(builder.Configuration);

var app = builder.Build();

//No início do pipeline, adiciona o middleware para lidar com os cabeçalhos encaminhados (Forwarded Headers)
//A requisição chega como http, mas o servidor está atrás de um proxy.
app.UseForwardedHeaders();

//Antes de quase tudo, para pegar logs e medir a requisição inteiramente.
app.UseSerilogRequestLogging();

//Pega a exceção de qualquer ponto abaixo e retorna um erro padronizado para o cliente, sem expor detalhes do servidor.
app.UseExceptionHandler();


//Somente em produção, força o uso de HTTPS e HSTS (HTTP Strict Transport Security) para melhorar a segurança.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Antes do rate limiter, para a 429 levar os headers de CORS. Sem eles o navegador esconde a
// resposta do front web. Também responde o preflight, que não leva token.
app.UseCors(ApiConfigurationsExtensions.CorsPolicy);

//Antes de autenticar, limita a taxa de requisições para evitar ataques de força bruta e DoS.
app.UseRateLimiter(); // Adiciona o middleware de limitação de taxa, que aplica as políticas de limitação de taxa definidas na configuração da API

//Definem quem é o usuário e suas permissões
app.UseAuthentication(); // Adiciona o middleware de autenticação, que valida os tokens JWT nas requisições
app.UseAuthorization(); // Adiciona o middleware de autorização, que verifica se o usuário autenticado tem permissão para acessar os recursos solicitados

//Mapeia os endpoints apenas em desenvolvimento, para não expor a documentação da API em produção.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); //Adiciona a rota /scalar/v1 para acessar a documentação da API
}

app.MapHealthChecks("/health").AllowAnonymous(); // Adiciona o endpoint de health check, que pode ser usado para monitoramento da aplicação. Permite acesso anônimo.

app.MapGroup("/api/auth")
    .RequireRateLimiting(ApiConfigurationsExtensions.AuthRateLimitingPolicy)
    .MapIdentityApi<IdentityUser<Guid>>();

app.MapControllers().RequireAuthorization(); //Mapeia os controllers da API e exige autorização para todos os endpoints, exceto os explicitamente marcados com [AllowAnonymous]

app.Run();

public partial class Program { } //Adicionado para permitir testes de integração com o WebApplicationFactory