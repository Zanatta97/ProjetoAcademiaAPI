using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Interfaces
{
    /// <summary>
    /// Representa o usuário atual da aplicação.
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// Obtém o ID do usuário atual.
        /// </summary>
        Guid UserId { get; }
    }
}
