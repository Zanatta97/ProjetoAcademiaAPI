using ProjetoAcademiaAPI.Api.Configuration;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Continua usando o Ilooger e o Serilog apenas assume a saída dos dados
builder.Services.AddSerilog((services, cfg) => cfg
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services));

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

//Mapeia os endpoints apenas em desenvolvimento, para não expor a documentação da API em produção.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); //Adiciona a rota /scalar/v1 para acessar a documentação da API
}

app.MapHealthChecks("/health").AllowAnonymous();

//app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { } //Adicionado para permitir testes de integração com o WebApplicationFactory