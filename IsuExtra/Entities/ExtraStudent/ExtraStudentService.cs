using System.Collections.Generic;
using System.Linq;
using Isu;
using IsuExtra.Entities;

namespace IsuExtra.Entities.ExtraStudent
{
    public class ExtraStudentService
    {
        public List<ExtraStudent> ExtraStudents { get; } = new List<ExtraStudent>();
        public ExtraStudent AddExtraStudent(Student student, List<Lesson> groupSchedule)
        {
            ExtraStudent newExtraStudent = new ExtraStudent(student, groupSchedule);
            ExtraStudents.Add(newExtraStudent);
            return newExtraStudent;
        }

        public List<ExtraStudent> GetStudentsWithoutOgnp()
        {
            return ExtraStudents.Where(student => student.Ognps.Count == 0).ToList();
        }
    }
}