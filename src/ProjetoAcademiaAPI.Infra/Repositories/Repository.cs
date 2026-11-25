using Microsoft.EntityFrameworkCore;
using ProjetoAcademiaAPI.Application.Interfaces.Repositories;
using ProjetoAcademiaAPI.Domain.Common;
using ProjetoAcademiaAPI.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Repositories
{
    /// <summary>
    /// Implementação genérica de <see cref="IRepository{T}"/> sobre o AppDbContext. Os repositórios
    /// específicos herdam daqui e acrescentam as consultas por ambiente.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os registros da entidade T do banco de dados de forma assíncrona.
        /// Sem Tracking para melhorar o desempenho em consultas somente leitura.
        /// </summary>
        /// <returns>IEnumerable<typeparamref name="T"/></returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(ct);
        }

        /// <summary>
        /// Retorna um registro da entidade T do banco de dados que corresponde ao predicado fornecido de forma assíncrona.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual async Task<T?> GetByIdAsync(long id, CancellationToken ct)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        /// <summary>
        /// Retorna um registro da entidade T do banco de dados que corresponde ao predicado fornecido de forma assíncrona, sem Tracking.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual async Task<T?> GetByIdReadOnlyAsync(long id, CancellationToken ct)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        /// <inheritdoc />
        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        /// <inheritdoc />
        public virtual void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        /// <inheritdoc />
        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
