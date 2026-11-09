namespace ProjetoAcademiaAPI.Api.Configuration
{
    /// <summary>
    /// Classe para definições das políticas de CORS da aplicação
    /// </summary>
    public sealed class AppCorsOptions
    {
        /// <summary>
        /// Define o nome da seção nas configurações
        /// </summary>
        public const string SectionName = "Cors";

        /// <summary>
        /// Define as origens permitidas para as requisições.
        /// </summary>
        public string[] AllowedOrigins { get; init; } = [];
    }
}
