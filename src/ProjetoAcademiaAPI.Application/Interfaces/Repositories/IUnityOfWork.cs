using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Interfaces.Repositories
{
    /// <summary>
    /// Agrupa os repositórios sobre um único DbContext (Scoped, um por requisição): as alterações feitas
    /// por qualquer repositório vão ao banco juntas, numa única transação, em <see cref="SaveChangesAsync"/>.
    /// </summary>
    public interface IUnityOfWork : IDisposable
    {
        //TODO: Adicionar todos os repositórios aqui, para que possam ser injetados no controller.
        IApiLogRepository ApiLogRepository { get; }

        /// <summary>Grava no banco tudo o que foi marcado nos repositórios desde o último save.</summary>
        /// <returns>Quantidade de linhas afetadas.</returns>
        Task<int> SaveChangesAsync();
    }
}

