using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Common
{
    /// <summary>
    /// Entidade base criada para que as outras entidades não repitam código.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// ID da entidade gerado sequencialmente
        /// </summary>
        public long Id { get; protected set; }
    }
}
