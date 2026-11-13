using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProjetoAcademiaAPI.Api.Logging;
using ProjetoAcademiaAPI.Api.Services;
using ProjetoAcademiaAPI.Application.Interfaces;
using ProjetoAcademiaAPI.Infra.Context;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using static System.Net.WebRequestMethods;

namespace ProjetoAcademiaAPI.Api.Configuration
{
    public static class ApiConfigurationsExtensions
    {
        public const string CorsPolicy = "AllowFrontend";
        public const string AuthRateLimitingPolicy = "auth";

        public static IServiceCollection AddApiConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatedOptions<AppCorsOptions>(AppCorsOptions.SectionName);
            services.AddValidatedOptions<AuthRateLimitOptions>(AuthRateLimitOptions.SectionName);

            services.AddHealthChecks();
            services.AddApiWeb();            
            services.AddApiCors(configuration);
            services.AddApiForwardedHeaders();
            services.AddApiIdentity(configuration);
            services.AddApiRateLimiter();
            services.AddHttpContextAccessor();

            //Vincula os serviços com as interfaces correspondentes para injeção de dependência
            services.AddScoped<ICurrentUser, CurrentUserService>();


            return services;
        }

        /// <summary>
        /// Registra as opções validadas para o tipo especificado.
        /// </summary>
        /// <remarks>As opções são vinculadas à seção de configuração especificada,
        /// validadas usando anotações de dados e validadas quando o aplicativo é iniciado.</remarks>
        /// <typeparam name="T">O Tipo de Opções.</typeparam>
        /// <param name="services">A coleção de serviços à qual as opções são adicionadas.</param>
        /// <param name="section">A seção de configuração usada para vincular as opções.</param>
        private static void AddValidatedOptions<T>(this IServiceCollection services, string section) where T : class =>
            services.AddOptions<T>()
            .BindConfiguration(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();


        /// <summary>
        /// Adiciona as configurações da API web.
        /// </summary>
        /// <param name="services">A coleção de serviços à qual os serviços da API web são adicionados.</param>
        private static void AddApiWeb(this IServiceCollection services)
        {
            //Adiciona os controllers da API e configura o serializador JSON para converter enums em strings
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            //Adiciona o middleware de tratamento de exceções e personaliza os detalhes do problema para incluir o traceId
            services.AddProblemDetails(options => options.CustomizeProblemDetails = 
                context => context.ProblemDetails.Extensions["traceId"] = context.HttpContext.GetTraceId());

            //Adiciona a documentação da API usando OpenAPI/Swagger
            services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());
        }

        /// <summary>
        /// Adiciona as configurações de CORS da API.
        /// </summary>
        /// <param name="services">A coleção de serviços à qual os serviços de CORS são adicionados.</param>
        /// <param name="configuration">A configuração que contém as origens CORS permitidas.</param>
        private static void AddApiCors(this IServiceCollection services, IConfiguration configuration)
        {
            // Obtém as origens permitidas da configuração
            var origins = configuration.GetSection(AppCorsOptions.SectionName).Get<AppCorsOptions>()?.AllowedOrigins ?? [];


            // Adiciona a política de CORS com as origens permitidas, permitindo qualquer cabeçalho e método
            services.AddCors(options =>
                options.AddPolicy(CorsPolicy, policy =>
                    policy.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()));

        }


        /// <summary>
        /// Configura os cabeçalhos encaminhados da API. Permite que a aplicação aceite cabeçalhos de encaminhamento (como X-Forwarded-For e X-Forwarded-Proto) de qualquer rede ou proxy.
        /// </summary>
        /// <param name="services">A coleção de serviços à qual os serviços de cabeçalhos encaminhados são adicionados.</param>
        private static void AddApiForwardedHeaders(this IServiceCollection services)
        {
            // Adiciona o suporte para cabeçalhos encaminhados (X-Forwarded-For, X-Forwarded-Proto, etc.)
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownIPNetworks.Clear(); // Limpa as redes conhecidas para aceitar cabeçalhos de qualquer rede
                options.KnownProxies.Clear();  // Limpa os proxies conhecidos para aceitar cabeçalhos de qualquer proxy
            });
        }

        /// <summary>
        /// Registra os serviços de identidade e autorização da API, 
        /// incluindo usuários e funções de identidade com armazenamento do Entity Framework, 
        /// endereços de e-mail exclusivos, requisitos de senha, expiração de token portador,
        /// expiração de token de atualização e a política de autorização apenas para administradores.
        /// </summary>
        /// <param name="services">A coleção de serviços à qual os serviços de identidade e autorização são adicionados.</param>
        /// <param name="configuration">A configuração que contém as definições de expiração de token.</param>
        private static void AddApiIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            // Adiciona os serviços de identidade com usuários e funções, usando o Entity Framework para armazenamento
            services.AddIdentityApiEndpoints<IdentityUser<Guid>>(options => {
                options.Password.RequiredLength = 8; // Define o comprimento mínimo da senha como 8 caracteres
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>() // Adiciona suporte para funções de identidade
            .AddEntityFrameworkStores<AppDbContext>(); // Adiciona o armazenamento do Entity Framework para usuários e funções

            // Configura as opções de expiração de token portador e token de atualização
            services.Configure<BearerTokenOptions>(IdentityConstants.BearerScheme, options =>
            {
                options.BearerTokenExpiration = TimeSpan.FromMinutes(configuration.GetValue("Auth:BearerTokenMinutes", 60));
                options.RefreshTokenExpiration = TimeSpan.FromDays(configuration.GetValue("Auth:RefreshTokenDays", 30));
            });

            // Adiciona a política de autorização apenas para administradores
            services.AddAuthorizationBuilder()
                .AddPolicy(AuthorizationPolicies.AdminOnly, policy => policy.RequireRole("Admin"));
        }

        /// <summary>
        /// Configura o limitador de taxa da API para solicitações de autenticação, particionadas por endereço IP remoto.
        /// As solicitações rejeitadas retornam HTTP 429 (Too Many Requests), incluem um cabeçalho Retry-After quando disponível
        /// e escrevem uma resposta de detalhes do problema.
        /// </summary>
        /// <param name="services">A coleção de serviços a ser configurada.</param>
        private static void AddApiRateLimiter(this IServiceCollection services) =>
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // Define o código de status HTTP para rejeição como 429 (Too Many Requests)

                // Adiciona uma política de limitação de taxa para solicitações de autenticação,
                // particionadas por endereço IP remoto
                options.AddPolicy(AuthRateLimitingPolicy, context =>
                {
                    // Obtém as opções de limitação de taxa da configuração
                    var limit = context.RequestServices.GetRequiredService<IOptions<AuthRateLimitOptions>>().Value;

                    // Retorna um limitador de taxa de janela fixa para o endereço IP remoto da solicitação
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = limit.PermitLimit, // Define o número máximo de solicitações permitidas dentro da janela de tempo
                            Window = TimeSpan.FromSeconds(limit.WindowSeconds), // Define a duração da janela de tempo para a limitação de taxa
                            QueueLimit = 0 // Define o limite da fila como 0, ou seja, não permite enfileiramento de solicitações
                        });
                });

                // Define o comportamento quando uma solicitação é rejeitada devido à limitação de taxa
                options.OnRejected = async (context, ct) =>
                {
                    var http = context.HttpContext; // Obtém o contexto HTTP da solicitação rejeitada

                    // Se o limitador de taxa fornecer um cabeçalho Retry-After, adiciona-o à resposta HTTP
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                        http.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();

                    // Obtém o serviço de detalhes do problema e
                    // escreve uma resposta de detalhes do problema para a solicitação rejeitada
                    var problemDetails = http.RequestServices.GetRequiredService<IProblemDetailsService>();
                    await problemDetails.TryWriteAsync(new ProblemDetailsContext
                    {
                        HttpContext = http,
                        ProblemDetails = new()
                        {
                            Status = StatusCodes.Status429TooManyRequests,
                            Title = "Muitas tentativas.",
                            Detail = "Tente novamente em alguns instantes."
                        }
                    });
                };
            });
    }
}
