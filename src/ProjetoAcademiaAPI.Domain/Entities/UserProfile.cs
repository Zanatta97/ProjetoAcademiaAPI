using ProjetoAcademiaAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public class UserProfile : IUserOwned
    {
        public Guid UserId { get; private set; }
    }
}
