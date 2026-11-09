using ProjetoAcademiaAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    /// <summary>
    /// Entidade para gravação dos logs no banco de dados.
    /// </summary>
    public class ApiLog : EntityBase
    {
        public const int MaxTraceIdLength = 1024;
        public const int MaxMethodLength = 128;
        public const int MaxUrlLength = 2048;

        public string TraceId { get; private set; } = null!;
        public DateTimeOffset OcurredAt { get; private set; }
        public string Method { get; private set; } = null!;
        public string Path { get; private set; } = null!;
        public string? QueryString { get; private set; }
        public int StatusCode { get; private set; }
        public long ElapsedMs { get; private set; }
        public string? UserId { get; private set; }
        public string? RequestBody { get; private set; }
        public string? ResponseBody { get; private set; }
        public string? ExceptionMessage { get; private set; }
        public string? StackTrace { get; private set; }
        public bool IsError { get; private set; }

        private ApiLog() { }

        public static ApiLog Create(string traceId, DateTimeOffset ocurredAt, string method, 
                                    string path, string? queryString, int statusCode, long elapsedMs,
                                    string? userId, string? requestBody, string? responseBody,
                                    Exception? exception)
        { 
            return new ApiLog
            {
                TraceId = Truncate(traceId, MaxTraceIdLength),
                OcurredAt = ocurredAt,
                Method = Truncate(method, MaxMethodLength),
                Path = Truncate(path, MaxUrlLength),
                QueryString = queryString,
                StatusCode = statusCode,
                ElapsedMs = elapsedMs,
                UserId = userId,
                RequestBody = requestBody,
                ResponseBody = responseBody,
                ExceptionMessage = exception?.Message,
                StackTrace = exception?.StackTrace,
                IsError = exception != null || statusCode >= 400
            };
        }

        /// <summary>
        /// Método para truncar os registros criados de log para não lotar o banco de dados
        /// </summary>
        /// <param name="value">Informação a ser truncada</param>
        /// <param name="maxLength">Tamanho máximo da informação, excesso será truncado</param>
        /// <returns></returns>
        private static string Truncate(string? value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

    }
}
