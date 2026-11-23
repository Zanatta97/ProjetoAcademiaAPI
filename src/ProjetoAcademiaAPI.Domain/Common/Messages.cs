using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Common
{
    public static class Messages
    {
        public const string WeeklyCardioGoalMinutesCannotBeNegative = "Meta de Cardio Semanal não pode ser negativa.";
        public const string DefaultRestTimeSecondsCannotBeNegative = "O tempo de descanso padrão não pode ser negativo.";
        public const string TimeZoneCannotBeNullOrEmpty = "O fuso horário não pode ser nulo ou vazio.";
        public static string InvalidTimeZone(string timeZone) => $"Fuso horário inválido: {timeZone}";
        public const string UpdateGlobalExerciseNotAllowed = "Não é permitido alterar um exercício padrão.";
        public const string MaxRepsCannotBeLessThanMinReps = "O número máximo de repetições não pode ser menor que o número mínimo de repetições.";
        public const string DropSetRequiresDropTargetReps = "Para um Drop Set, é necessário fornecer os valores de repetições para cada drop.";
        public const string RestTimeSecondsCannotBeNegative = "O tempo de descanso não pode ser negativo.";
        public const string StartTimeCannotBeAfterEndTime = "A data de início não pode ser posterior à data de término.";

    }
}
