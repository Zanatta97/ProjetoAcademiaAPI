using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Interfaces
{
    public interface IApiLogSwitch
    {
        /// <summary>
        /// Define se o log está ativo ou não
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Altera o estado log conforme parâmetro passado.
        /// </summary>
        /// <param name="enabled">Booleano para definir se o log está ativo ou não</param>
        void Set(bool enabled);
    }
}
