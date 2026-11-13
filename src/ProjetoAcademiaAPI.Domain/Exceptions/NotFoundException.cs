using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Exceptions
{
    /// <summary>
    /// Representa uma exceção para quando um recurso não é encontrado.
    /// </summary>
    public sealed class NotFoundException : DomainException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
