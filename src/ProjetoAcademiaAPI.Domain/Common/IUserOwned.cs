using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Common
{
    /// <summary>
    /// Interface criada para passar o ID do usuário ao qual o objeto pertence e evitar repetição de código.
    /// </summary>
    public interface IUserOwned
    {
        string UserId { get; }
    }
}
