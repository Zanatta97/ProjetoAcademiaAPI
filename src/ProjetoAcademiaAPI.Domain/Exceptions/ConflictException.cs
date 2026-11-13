using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Exceptions
{
    /// <summary>
    /// Representa uma exceção para quando há um conflito de dados.
    /// </summary>
    public sealed class ConflictException : DomainException
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
