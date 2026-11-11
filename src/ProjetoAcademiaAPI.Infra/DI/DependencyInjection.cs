using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoAcademiaAPI.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.DI
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra o banco de dados da aplicação com SQL Server, com snake_case
        /// e configuração de retry em caso de falha de conexão.
        /// </summary>
        /// <param name="services">A coleção de serviços a ser configurada.</param>
        /// <param name="configuration">A configuração da aplicação contendo a string de conexão com o banco de dados.</param>
        /// <returns>The configured service collection.</returns>
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options
            .UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(15),
                    errorNumbersToAdd: null))
            .UseSnakeCaseNamingConvention());
            return services;
        }
    }
}
