using System.Diagnostics;

namespace ProjetoAcademiaAPI.Api.Logging
{
    internal static class HttpContextLogExtensions
    {
        public static string GetTraceId(this HttpContext context) =>
            Activity.Current?.Id ?? context.TraceIdentifier;
    }
}
