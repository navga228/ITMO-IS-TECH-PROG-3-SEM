using System;
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

            if (groupName.Length != 5)
            {
                throw new IsuException("Invalid name to group");
            }

            char derectionLetter = char.Parse(groupName.Substring(0, 1));
            int function = 0;
            int courseNumber = 0;
            int groupNumber = 0;

            // FormatException
            if (!int.TryParse(groupName.Substring(2, 1), out courseNumber)
                || !int.TryParse(groupName.Substring(3, 2), out groupNumber)
                || !int.TryParse(groupName.Substring(1, 1), out function))
            {
                throw new IsuException("Invalid name to group");
            }

            courseNumber = int.Parse(groupName.Substring(2, 1));
            if (groupName.Length != 5 || !char.IsLetter(derectionLetter) || courseNumber > 4 || courseNumber == 0 || (function != 3 && function != 4))
                throw new IsuException("Invalid name to group test");
            CourseNumber = int.Parse(groupName.Substring(2, 1));
            GroupNumber = int.Parse(groupName.Substring(3, 2));
            Function = int.Parse(groupName.Substring(1, 1));
            DerectionLetter = derectionLetter;
            GroupName = groupName;
        }

        public List<Student> Students
        {
            get => _students;
        }

        public int GroupNumber { get; }
        public int CourseNumber { get; }
        public string GroupName { get; }
        public char DerectionLetter { get; }
        public int Function { get; }
    }
}
