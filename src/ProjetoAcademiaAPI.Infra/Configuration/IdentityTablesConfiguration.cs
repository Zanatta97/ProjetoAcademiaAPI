using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Configuration
{
    /// <summary>
    /// Configura as tabelas do Identity para o Entity Framework Core, definindo o mapeamento correto.
    /// Necessário para manter o padrão de nomenclatura em snake_case.
    /// </summary>
    public sealed class IdentityTablesConfiguration : IEntityTypeConfiguration<IdentityUser<Guid>>,
        IEntityTypeConfiguration<IdentityUserClaim<Guid>>,
        IEntityTypeConfiguration<IdentityUserLogin<Guid>>,
        IEntityTypeConfiguration<IdentityUserRole<Guid>>,
        IEntityTypeConfiguration<IdentityUserToken<Guid>>,
        IEntityTypeConfiguration<IdentityRoleClaim<Guid>>,
        IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUser<Guid>> builder)
        {
            builder.ToTable("asp_net_users");

            builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("user_name_index");
            builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("email_index");
        }
        public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
        {
            builder.ToTable("asp_net_user_claims");
        }
        public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
        {
            builder.ToTable("asp_net_user_logins");
        }

        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            builder.ToTable("asp_net_user_roles");
        }

        public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
        {
            builder.ToTable("asp_net_user_tokens");
        }

        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
        {
            builder.ToTable("asp_net_role_claims");
        }

        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.ToTable("asp_net_roles");

            builder.HasIndex(r => r.NormalizedName).HasDatabaseName("role_name_index");
        }
    }
}