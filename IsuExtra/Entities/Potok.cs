using System.Collections.Generic;
using IsuExtra.Entities.Ognp;

namespace IsuExtra.Entities
{
    public class Potok
    {
        private string potokName;
        public Potok(string name, List<Lesson> potokSchedule, int maxStudentInPotok)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new OgnpException("Name is null or empty");
            }

            if (potokSchedule == null)
            {
                throw new OgnpException("Potok schedule is null");
            }

            potokName = name;
            PotokSchedule = potokSchedule;
            Students = new List<ExtraStudent.ExtraStudent>();
            MaxStudentInPotok = maxStudentInPotok;
        }

        public List<Lesson> PotokSchedule { get; }
        public List<ExtraStudent.ExtraStudent> Students { get; }
        public int MaxStudentInPotok { get; }
    }
}