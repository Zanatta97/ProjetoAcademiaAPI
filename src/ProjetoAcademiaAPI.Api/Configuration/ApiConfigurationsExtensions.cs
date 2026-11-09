using Microsoft.AspNetCore.HttpOverrides;
using ProjetoAcademiaAPI.Api.Logging;
using System.Text.Json.Serialization;

namespace ProjetoAcademiaAPI.Api.Configuration
{
    public static class ApiConfigurationsExtensions
    {
        public const string CorsPolicy = "AllowFrontend";

        public static IServiceCollection AddApiConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatedOptions<AppCorsOptions>(AppCorsOptions.SectionName);

            services.AddHealthChecks();
            services.AddApiWeb();
            services.AddApiCors(configuration);
            services.AddApiForwardedHeaders();


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
            services.AddOpenApi();
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

    }
}
