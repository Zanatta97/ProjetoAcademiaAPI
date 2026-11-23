using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Enums
{
    /// <summary>
    /// Especifica os dias da semana como uma enumeração de bandeiras (flags).
    /// </summary>
    /// <remarks>Valores podem ser combinados usando operações bit a bit para representar vários dias.</remarks>
    public enum DaysOfWeek
    {
        None = 0,
        Sunday = 1 << 0, //representa o valor 00000001
        Monday = 1 << 1, //representa o valor 00000010
        Tuesday = 1 << 2, //representa o valor 00000100
        Wednesday = 1 << 3, //representa o valor 00001000
        Thursday = 1 << 4, //representa o valor 00010000
        Friday = 1 << 5, //representa o valor 00100000
        Saturday = 1 << 6 //representa o valor 01000000
    }
}
