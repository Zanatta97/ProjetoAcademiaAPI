using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Text.Json;

namespace ProjetoAcademiaAPI.Api.Configuration
{
    /// <summary>
    /// Adiciona um esquema de segurança Bearer e o aplica a cada operação no documento OpenAPI.
    /// </summary>
    public class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
    {
        private const string SchemeName = "Bearer";

        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Components ??= new OpenApiComponents();// Inicializa os componentes do documento OpenAPI, se ainda não estiverem definidos
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>(); // Inicializa os esquemas de segurança do documento OpenAPI, se ainda não estiverem definidos
            // Adiciona um esquema de segurança Bearer ao documento OpenAPI
            document.Components.SecuritySchemes[SchemeName] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
            };

            // Aplica o esquema de segurança Bearer a cada operação no documento OpenAPI
            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations!.Values))
            {
                operation.Security ??= []; // Inicializa a lista de requisitos de segurança da operação, se ainda não estiver definida

                // Adiciona um requisito de segurança que referencia o esquema de segurança Bearer
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(SchemeName, document)] = []
                });
            }

            return Task.CompletedTask;
        }
    }
}
