using Isu.Services;

namespace Isu
{
    public class Student
    {
        private static int _nextID;
        public Student()
        {
            StudName = null;
            StudGroup = null;
            StudID = _nextID;
            _nextID++;
        }

        public Student(string name, Group gr)
        {
            StudName = name;
            StudID = _nextID;
            _nextID++;
            StudGroup = gr;
        }

        public string StudName { get; set; }
        public int StudID { get; }
        public Group StudGroup { get; set; }
    }
}