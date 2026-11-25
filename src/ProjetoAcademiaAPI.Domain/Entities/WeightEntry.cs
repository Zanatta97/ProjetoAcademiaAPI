using ProjetoAcademiaAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public class WeightEntry : EntityBase, IUserOwned
    {
        public Guid UserId { get; internal set; }
        public DateTimeOffset EntryDate { get; internal set; }
        public decimal Weight { get; internal set; }
    }
}
