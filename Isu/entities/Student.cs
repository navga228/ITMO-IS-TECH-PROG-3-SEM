using Isu.Services;

namespace Isu
{
    public class Student
    {
        private static int _nextId;

        public Student(string name, Group group)
        {
            StudentName = name;
            StudentId = _nextId;
            _nextId++;
            StudentGroup = group;
        }

        public string StudentName { get; }
        public int StudentId { get; }
        public Group StudentGroup { get; set; }
    }
}