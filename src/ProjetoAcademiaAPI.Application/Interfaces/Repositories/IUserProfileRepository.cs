using ProjetoAcademiaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Interfaces.Repositories
{
    public interface IUserProfileRepository
    {
        /// <summary>
        /// Retorna todos os registros da entidade, sem filtro e sem rastreamento
        /// </summary>
        public Task<IEnumerable<UserProfile>> GetAllAsync(CancellationToken ct);

        //TODO: Implementar paginação
        //public Task<PagedResponse<T>> GetPagedAsync(PaginationParameters paging, CancellationToken ct);

        /// <summary>
        /// Primeiro registro que atende ao predicado, rastreado pelo EF (use para alterar ou excluir).
        /// </summary>
        /// <param name="userId">ID do usuário a ser buscado</param>
        /// <returns>O registro, ou null se nenhum atender.</returns>
        public Task<UserProfile?> GetByIdAsync(Guid userId, CancellationToken ct);

        /// <summary>
        /// Primeiro registro que atende ao predicado, sem rastreamento (use para leitura apenas).
        /// </summary>
        /// <param name="userId">ID do usuario a ser buscado</param>
        /// <returns>O registro, ou null se nenhum atender.</returns>
        public Task<UserProfile?> GetByIdReadOnlyAsync(Guid userId, CancellationToken ct);

        /// <summary>Marca a entidade para inclusão.</summary>
        public void Add(UserProfile entity);

        /// <summary>Marca a entidade inteira como alterada.</summary>
        public void Update(UserProfile entity);

        /// <summary>Marca a entidade para exclusão.</summary>
        public void Delete(UserProfile entity);
    }
}
