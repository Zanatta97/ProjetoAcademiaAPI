using ProjetoAcademiaAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public sealed class UserProfile : IUserOwned
    {
        public Guid UserId { get; private set; }
        public int? HeightCm { get; private set; }
        public int WeeklyCardioGoalMinutes { get; private set; } = 0;
        public int DefaultRestTimeSeconds { get; private set; } = 90;
        public string TimeZone { get; private set; } = "America/Sao_Paulo";

        private UserProfile() { }

        public UserProfile(Guid userId)
        {
            UserId = userId;
        }

        public void Update(int? heightCm, int weeklyCardioGoalMinutes, int defaultRestTimeSeconds, string timeZone)
        {
            HeightCm = heightCm;

            if (weeklyCardioGoalMinutes < 0)
            {
                throw new ArgumentException(Messages.WeeklyCardioGoalMinutesCannotBeNegative, nameof(weeklyCardioGoalMinutes));
            }

            WeeklyCardioGoalMinutes = weeklyCardioGoalMinutes;

            if (defaultRestTimeSeconds < 0)
            {
                throw new ArgumentException(Messages.DefaultRestTimeSecondsCannotBeNegative, nameof(defaultRestTimeSeconds));
            }

            DefaultRestTimeSeconds = defaultRestTimeSeconds;
            
            if (string.IsNullOrWhiteSpace(timeZone))
            {
                throw new ArgumentException(Messages.TimeZoneCannotBeNullOrEmpty, nameof(timeZone));
            } else if (!TimeZoneInfo.GetSystemTimeZones().Any(tz => tz.Id.Equals(timeZone, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException(String.Format(Messages.InvalidTimeZone(timeZone)), nameof(timeZone));
            }

            TimeZone = timeZone;
        }
    }
}
