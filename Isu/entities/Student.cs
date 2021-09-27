using Isu.Services;

namespace Isu
{
    public class Student
    {
        public Student()
        {
            StudName = null;
            StudGroup = null;
            IsuService.IDcounter++;
            StudID = IsuService.IDcounter;
        }

        public Student(string name, int id, Group gr)
        {
            StudName = name;
            StudID = id;
            StudGroup = gr;
        }

        public string StudName { get; set; }
        public int StudID { get; set; }
        public Group StudGroup { get; set; }
    }
}