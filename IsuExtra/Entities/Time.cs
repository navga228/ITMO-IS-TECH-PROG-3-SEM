using System;
using System.Linq;
using IsuExtra.Entities.Ognp;

namespace IsuExtra.Entities
{
    public class Time
    {
        public Time(int hour, int minutes, string dayOfWeek)
        { // Проверка на соответсвие дню недели
            string[] daysOfWeek = { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
            if (string.IsNullOrEmpty(dayOfWeek))
            {
                throw new OgnpException("Day of week is null or empty");
            }

            string lowerDayOfWeek = dayOfWeek.ToLower();
            if (!daysOfWeek.Contains(lowerDayOfWeek))
            {
                throw new OgnpException("Invalid day of week");
            }

            Hour = hour;
            Minutes = minutes;
            DayOfWeek = lowerDayOfWeek;
        }

        public int Hour { get; }
        public int Minutes { get; }
        public string DayOfWeek { get; }
    }
}