using System;
using System.Linq;
using IsuExtra.Entities.Ognp;

namespace IsuExtra.Entities
{
    public class Time
    {
        public Time(int hour, int minutes, DaysOfWeek dayOfWeek)
        {
            Hour = hour;
            Minutes = minutes;
            DayOfWeek = dayOfWeek;
        }

        public enum DaysOfWeek
        {
            /// <summary>The first value</summary>
            Monday,

            /// <summary>The second value</summary>
            Tuesday,

            /// <summary>The third value</summary>
            Wednesday,

            /// <summary>The fourth value</summary>
            Thursday,

            /// <summary>The fifth value</summary>
            Friday,

            /// <summary>The sixth value</summary>
            Saturday,

            /// <summary>The seventh value</summary>
            Sunday,
        }

        public int Hour { get; }
        public int Minutes { get; }
        public Enum DayOfWeek { get; }
    }
}