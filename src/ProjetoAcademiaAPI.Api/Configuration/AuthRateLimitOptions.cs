using System.ComponentModel.DataAnnotations;

namespace ProjetoAcademiaAPI.Api.Configuration
{
    public sealed class AuthRateLimitOptions
    {
        public const string SectionName = "RateLimit:Auth";

        /// <summary>
        /// Define o valor máximo de solicitações permitidas por janela de tempo para autenticação.
        /// </summary>
        /// <remarks>Valores válidos variam de 0 a 10.000. O valor padrão é 10.</remarks>
        [Range(0, 10_000)]
        public int PermitLimit { get; init; } = 10;

        /// <summary>
        /// Define a duração da janela de tempo em segundos para autenticação.
        /// </summary>
        /// <remarks>Valores válidos variam de 1 a 3.600. O valor padrão é 60.</remarks>
        [Range(1, 3_600)]
        public int WindowSeconds { get; init; } = 60;
    }
}
