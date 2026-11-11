using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoAcademiaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Context
{
    public sealed class AppDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        //Define as entidades que serão mapeadas para o banco de dados, incluindo a tabela de logs da API.
        public DbSet<ApiLog> ApiLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Chamada do método base para configurar o modelo de identidade padrão

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // Aplica as configurações de entidade do assembly atual

        }
    }
}
