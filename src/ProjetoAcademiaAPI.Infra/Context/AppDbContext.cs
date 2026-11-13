using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoAcademiaAPI.Application.Interfaces;
using ProjetoAcademiaAPI.Domain.Common;
using ProjetoAcademiaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Data.SqlClient;
using ProjetoAcademiaAPI.Domain.Exceptions;
using ProjetoAcademiaAPI.Infra.Context.Errors;

namespace ProjetoAcademiaAPI.Infra.Context
{
    public sealed class AppDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IDbErrorClassifier _dbErrorClassifier;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUser currentUser, IDbErrorClassifier dbErrorClassifier) : base(options)
        { 
            _currentUser = currentUser; // Armazena a instância de ICurrentUser para uso posterior
            _dbErrorClassifier = dbErrorClassifier; // Armazena a instância de IDbErrorClassifier para uso posterior
        }

        private Guid CurrentUserId => _currentUser.UserId; // Obtém o ID do usuário atual a partir da instância de ICurrentUser

        //Define as entidades que serão mapeadas para o banco de dados, incluindo a tabela de logs da API.
        public DbSet<ApiLog> ApiLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Chamada do método base para configurar o modelo de identidade padrão

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // Aplica as configurações de entidade do assembly atual
            
            // Aplica filtros globais para entidades que implementam IUserOwned, garantindo que apenas os registros pertencentes ao usuário atual sejam retornados nas consultas.
            ApplyUserFilters(builder);


            //Versão do filtro caso fosse utilizado reflexão, porém como existem filtros diferentes,
            //como exemplo o de exercícios foi optado por criar um método específico para cada entidade,
            //Aplica filtros globais para entidades que implementam a interface IUserOwned,
            // garantindo que apenas os registros pertencentes ao usuário atual sejam retornados nas consultas.
            /*foreach (var entityType in builder.Model.GetEntityTypes()
                .Where(t => typeof(IUserOwned).IsAssignableFrom(t.ClrType)))
            {
                GetType()
                    .GetMethod(nameof(ApplyUserFilter), BindingFlags.NonPublic | BindingFlags.Instance) // Obtém o método ApplyUserFilter usando reflexão
                    ?.Invoke(this, [builder]);
            }*/

        }


        /*Versão do filtro caso fosse utilizado reflexão, porém como existem filtros diferentes, 
         * como exemplo o de exercícios foi optado por criar um método específico para cada entidade, 
         * garantindo maior controle sobre os filtros aplicados.
         /// <summary>
        /// Aplica um filtro global para entidades que implementam IUserOwned, 
        /// garantindo que apenas os registros pertencentes ao usuário atual sejam retornados nas consultas.
        /// </summary>
        /// <typeparam name="T">O Tipo da entidade a ser configurada.</typeparam>
        /// <param name="builder">O model builder usado para configurar a entidade.</param>
        private void ApplyUserFilter<T>(ModelBuilder builder) where T : class, IUserOwned
        {
            builder.Entity<T>().HasQueryFilter(e => e.UserId == CurrentUserId); // Aplica um filtro global para entidades que implementam IUserOwned, garantindo que apenas os registros pertencentes ao usuário atual sejam retornados nas consultas.
        }*/

        /// <summary>
        /// Restringe as consultas aos registros do usuário autenticado.
        /// Toda entidade nova que pertença a um usuário precisa de uma linha aqui.
        /// </summary>
        private void ApplyUserFilters(ModelBuilder builder)
        {
            // Dados exclusivos do usuário
            builder.Entity<UserProfile>().HasQueryFilter(e => e.UserId == CurrentUserId);
            /*builder.Entity<Workout>().HasQueryFilter(e => e.UserId == CurrentUserId);
            builder.Entity<WorkoutExercise>().HasQueryFilter(e => e.UserId == CurrentUserId);
            builder.Entity<PlannedSet>().HasQueryFilter(e => e.UserId == CurrentUserId);
            builder.Entity<WorkoutSession>().HasQueryFilter(e => e.UserId == CurrentUserId);
            builder.Entity<SessionSet>().HasQueryFilter(e => e.UserId == CurrentUserId);

            // Catálogo: exercícios do sistema (UserId nulo) são visíveis para todos,
            // os personalizados só para o dono.
            builder.Entity<Exercise>().HasQueryFilter(e => e.UserId == null || e.UserId == CurrentUserId);*/

            // ApiLog: sem filtro. O acesso é barrado pela policy AdminOnly.
        }


        // A Application não enxerga EF Core, então a violação de índice único sai daqui como uma
        // exceção de domínio. Sendo assim foi criado um validador de exceções de banco de dados, que é injetado no contexto e usado para classificar as exceções.
        // É este overload que o SaveChangesAsync(CancellationToken) chama por dentro.
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (DbUpdateException ex) when (_dbErrorClassifier.IsUniqueVaiolation(ex))
            {
                throw new ConflictException("Registro duplicado.");
            }
        }
    }
}
