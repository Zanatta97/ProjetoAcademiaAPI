using Microsoft.EntityFrameworkCore;
using ProjetoAcademiaAPI.Application.Interfaces.Repositories;
using ProjetoAcademiaAPI.Domain.Entities;
using ProjetoAcademiaAPI.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        protected readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os registros da entidade T do banco de dados de forma assíncrona.
        /// Sem Tracking para melhorar o desempenho em consultas somente leitura.
        /// </summary>
        /// <returns>IEnumerable<typeparamref name="T"/></returns>
        public virtual async Task<IEnumerable<UserProfile>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Set<UserProfile>().AsNoTracking().ToListAsync(ct);
        }

        /// <summary>
        /// Retorna um registro da entidade T do banco de dados que corresponde ao predicado fornecido de forma assíncrona.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual async Task<UserProfile?> GetByIdAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Set<UserProfile>().FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }

        /// <summary>
        /// Retorna um registro da entidade T do banco de dados que corresponde ao predicado fornecido de forma assíncrona, sem Tracking.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual async Task<UserProfile?> GetByIdReadOnlyAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Set<UserProfile>().AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }

        /// <inheritdoc />
        public virtual void Add(UserProfile profile)
        {
            _context.Set<UserProfile>().Add(profile);
        }

        /// <inheritdoc />
        public virtual void Update(UserProfile profile)
        {
            _context.Set<UserProfile>().Update(profile);
        }

        /// <inheritdoc />
        public virtual void Delete(UserProfile profile)
        {
            _context.Set<UserProfile>().Remove(profile);
        }
    }
}
