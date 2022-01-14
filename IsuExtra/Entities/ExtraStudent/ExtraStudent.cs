using System.Collections.Generic;
using Isu;
using IsuExtra.Entities.Ognp;

namespace IsuExtra.Entities.ExtraStudent
{
    public class ExtraStudent
    {
        public ExtraStudent(Student student, List<Lesson> groupSchedule)
        {
            Student = student;
            StudentSchedule = groupSchedule;
            Ognps = new List<Ognp.Ognp>();
        }

        public List<Lesson> StudentSchedule { get; }
        public List<Ognp.Ognp> Ognps { get; }
        public Student Student { get; }
    }
}