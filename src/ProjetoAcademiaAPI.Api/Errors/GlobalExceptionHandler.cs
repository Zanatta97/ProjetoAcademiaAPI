using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjetoAcademiaAPI.Domain.Exceptions;

namespace ProjetoAcademiaAPI.Api.Errors
{
    /// <summary>
    /// Trata as exceções globais da aplicação, registrando logs e gerando respostas padronizadas de detalhes do problema.
    /// </summary>
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger)
        {
            _problemDetailsService = problemDetailsService;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {

            // Se a exceção for uma OperationCanceledException e a solicitação foi cancelada,
            // retorna true para indicar que a exceção foi tratada.
            if (exception is OperationCanceledException && context.RequestAborted.IsCancellationRequested)
            {
                return true;
            }

            // Cria um objeto ProblemDetails com base no tipo da exceção, definindo o status HTTP e o título apropriados.
            ProblemDetails problema = exception switch
            {
                // Trata exceções do tipo ValidationException, que ocorrem quando há falhas de validação de dados.
                ValidationException ex => new ValidationProblemDetails(
                    ex.Errors
                        .GroupBy(e => e.PropertyName) // Agrupa os erros por nome da propriedade.
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())) // Cria um dicionário de erros de validação, onde a chave é o nome da propriedade e o valor é um array de mensagens de erro.
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Dados inválidos"
                },

                // Trata exceções do tipo UnauthorizedAccessException, que ocorrem quando o acesso a um recurso é negado.
                UnauthorizedAccessException => new() { Status = StatusCodes.Status401Unauthorized, Title = "Não autorizado" },

                // Trata exceções do tipo NotFoundException, que ocorrem quando um recurso não é encontrado.
                NotFoundException => new() { Status = StatusCodes.Status404NotFound, Title = "Não encontrado" },

                // Trata exceções do tipo ConflictException, que ocorrem quando há um conflito de dados, como uma violação de chave única.
                ConflictException => new() { Status = StatusCodes.Status409Conflict, Title = "Conflito de dados" },

                // Trata exceções do tipo DomainException, que ocorrem quando uma regra de negócio é violada.
                DomainException => new() { Status = StatusCodes.Status400BadRequest, Title = "Regra de Negócio Violada" },

                // Para todas as outras exceções não tratadas, retorna um status HTTP 500 (Erro Interno do Servidor) com o título "Erro inesperado".
                _ => new() { Status = StatusCodes.Status500InternalServerError, Title = "Erro inesperado" }
            };

            // Se o status do problema for 500 (Erro Interno do Servidor), registra a exceção como um erro no log.
            if (problema.Status == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Erro inesperado");
            }

            // Define o status da resposta HTTP com base no status do problema.
            context.Response.StatusCode = problema.Status!.Value;

            // Tenta escrever os detalhes do problema na resposta HTTP usando o serviço ProblemDetailsService.
            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problema,
                Exception = exception
            });
        }
    }
}
