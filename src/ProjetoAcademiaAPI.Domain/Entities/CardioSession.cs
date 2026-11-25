using ProjetoAcademiaAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public class CardioSession : EntityBase, IUserOwned
    {
        public Guid UserId { get; internal set; }
        public DateTimeOffset StartTime { get; internal set; }
        public DateTimeOffset EndTime { get; internal set; }

    }
}
