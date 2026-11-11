using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Context
{
    public sealed class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(new IdentityRole<Guid>
            {
                Id = new Guid("1f0c3a7e-6f41-4d7a-9c39-0a7e2c5d8b11"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "7d9b1c52-3e0a-4b68-8f21-5c6a4e9d0f37"

            });

            builder.HasData(new IdentityRole<Guid>
            {
                Id = new Guid("2a1b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "3f2e1d4c-5b6a-7c8d-9e0f-1a2b3c4d5e6f"
            });
        }
    }
}
