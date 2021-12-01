using System.Collections.Generic;

namespace IsuExtra.Entities.Ognp
{
    public class Ognp
    {
        public Ognp(string courseName, List<Potok> potoks, char megaFacultyLetter)
        {
            if (string.IsNullOrEmpty(courseName))
            {
                throw new OgnpException("Course name is null or empty");
            }

            if (potoks == null)
            {
                throw new OgnpException("Potoks is null");
            }

            CourseName = courseName;
            Potoks = potoks;
            MegaFacultyLetter = megaFacultyLetter;
        }

        public char MegaFacultyLetter { get; }
        public List<Potok> Potoks { get; }
        public string CourseName { get; }
    }
}