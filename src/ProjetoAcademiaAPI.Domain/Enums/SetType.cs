using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Enums
{
    public enum SetType
    {
        WarmUp,
        Feeder,
        Working,
        Drop
    }

    public static class SetTypeExtensions
    {
        /// <summary>
        /// Determina se o tipo de conjunto deve ser contado como volume.
        /// Retorna verdadeiro para tipos de conjunto "Working" e "Drop", caso contrário, retorna falso.
        /// </summary>
        /// <param name="setType">O tipo de set a ser avaliado.</param>
        public static bool CountAsVolume(this SetType setType)
        {
            return setType == SetType.Working || setType == SetType.Drop;
        }
    }
}
