using ProjetoAcademiaAPI.Application.Interfaces.Repositories;
using ProjetoAcademiaAPI.Domain.Entities;
using ProjetoAcademiaAPI.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Repositories
{
    public class WeightEntryRepository : Repository<WeightEntry>, IWeightEntryRepository
    {
        public WeightEntryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
