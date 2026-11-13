using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Context.Errors
{
    public class SqlServerDbErrorClassifier : IDbErrorClassifier
    {
        /// <summary>
        /// Determina se a exceção de atualização do banco de dados é uma violação de unicidade,
        /// verificando se a exceção interna é uma SqlException com o número de erro 2601 ou 2627.
        /// </summary>
        /// <param name="ex">A exceção de atualização do banco de dados a ser inspecionada.</param>
        /// <returns>Retorna true se a exceção interna for uma SqlException com o número de erro 2601 ou 2627; caso contrário, retorna false.</returns>
        public bool IsUniqueVaiolation(DbUpdateException ex)
        {
            return ex.InnerException is SqlException { Number: 2601 or 2627 };
        }
    }
}
