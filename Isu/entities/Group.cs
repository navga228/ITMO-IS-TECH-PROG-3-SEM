using System.Collections.Generic;
using Isu;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        private List<Student> students = new List<Student>();

        public Group()
        {
            GroupNumber = -1;
            CourseNum = -1;
            Groupname = null;
        }

        public Group(string groupname)
        {
            string m3 = groupname.Substring(0, groupname.Length - 3);
            int courseNum = int.Parse(groupname.Substring(2, 1));
            if (groupname.Length != 5 || !m3.Equals("M3") || courseNum > 4)
                throw new IsuException("Invalid name to group");
            CourseNum = int.Parse(groupname.Substring(2, 1));
            GroupNumber = int.Parse(groupname.Substring(3, 2));
            Groupname = groupname;
            StudentCounter = 0;
        }

        public List<Student> Students1
        {
            get => students;
            set => students = value;
        }

        public int GroupNumber { get; }
        public int CourseNum { get; }
        public string Groupname { get; set; }
        public int StudentCounter { get; set; }
    }
}
