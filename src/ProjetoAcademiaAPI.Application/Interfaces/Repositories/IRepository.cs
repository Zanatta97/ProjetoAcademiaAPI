using ProjetoAcademiaAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface com comandos básicos de banco de dados para todos
    /// os repositories usarem e evitar repetição de código.
    /// </summary>
    /// <typeparam name="T">Entidade mapeada no AppDBContext</typeparam>
    public interface IRepository<T> where T: EntityBase
    {
        /// <summary>
        /// Retorna todos os registros da entidade, sem filtro e sem rastreamento
        /// </summary>
        public Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);


        //TODO: Implementar paginação
        //public Task<PagedResponse<T>> GetPagedAsync(PaginationParameters paging, CancellationToken ct);

        /// <summary>
        /// Primeiro registro com o id informado, rastreado pelo EF (use para alterar ou excluir).
        /// </summary>
        /// <param name="id">ID do registro a ser buscado</param>
        /// <returns>O registro, ou null se nenhum atender.</returns>
        public Task<T?> GetByIdAsync(long id, CancellationToken ct);

        /// <summary>
        /// Primeiro registro com o id informado, sem rastreamento (use para leitura apenas).
        /// </summary>
        /// <param name="id">ID do registro a ser buscado</param>
        /// <returns>O registro, ou null se nenhum atender.</returns>
        public Task<T?> GetByIdReadOnlyAsync(long id, CancellationToken ct);

        /// <summary>Marca a entidade para inclusão.</summary>
        public void Add(T entity);

        /// <summary>Marca a entidade inteira como alterada.</summary>
        public void Update(T entity);

        /// <summary>Marca a entidade para exclusão.</summary>
        public void Delete(T entity);

    }
}
