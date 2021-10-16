using System.Collections.Generic;
using Isu;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        private List<Student> _students = new List<Student>();

        public Group(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new IsuException("Name of group is null or empty");
            }

            string m3 = groupName.Substring(0, groupName.Length - 3);
            int courseNum = int.Parse(groupName.Substring(2, 1));
            if (groupName.Length != 5 || !m3.Equals("M3") || courseNum > 4)
                throw new IsuException("Invalid name to group");
            CourseNumber = int.Parse(groupName.Substring(2, 1));
            GroupNumber = int.Parse(groupName.Substring(3, 2));
            GroupName = groupName;
        }

        public List<Student> Students
        {
            get => _students;
        }

        public int GroupNumber { get; }
        public int CourseNumber { get; }
        public string GroupName { get; }
    }
}
