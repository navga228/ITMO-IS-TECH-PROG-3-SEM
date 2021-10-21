using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private List<Group> groups = new List<Group>();
        private int maxStudentInGroup = 25;

        public Group AddGroup(string name)
        {
            Group newGroup = new Group(name);
            groups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            if (groups.Contains(group))
            {
                if (group.Students.Count >= maxStudentInGroup)
                {
                    throw new IsuException("The limit of students for the group has been exceeded");
                }

                Student stu = new Student(name, group);
                group.Students.Add(stu);
                return stu;
            }

            throw new IsuException("No such group exists");
        }

        public Student GetStudent(int id)
        {
            Student ans = groups.SelectMany(i => i.Students.Where(j => j.StudentId == id)).FirstOrDefault();
            if (ans != null)
            {
                return ans;
            }

            throw new IsuException("No such student exist");
        }

        public Student FindStudent(string name)
        {
            Student ans = groups.SelectMany(i => i.Students.Where(student => student.StudentName == name)).FirstOrDefault();
            if (ans != null)
            {
                return ans;
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new IsuException("Name of group is null or empty");
            }

            if (groupName.Length != 5)
            {
                throw new IsuException("Invalid name to group");
            }

            int courseNumber = 0;
            int groupNumber = 0;

            // FormatException
            if (!int.TryParse(groupName.Substring(2, 1), out courseNumber) || !int.TryParse(groupName.Substring(3, 2), out groupNumber))
            {
                throw new IsuException("Invalid name to group");
            }

            courseNumber = int.Parse(groupName.Substring(2, 1));
            groupNumber = int.Parse(groupName.Substring(3, 2));
            var selectedStudents = groups.Where(group => group.GroupNumber == groupNumber)
                        .Where(group => group.CourseNumber == courseNumber).Select(stud => stud.Students).FirstOrDefault();
            if (selectedStudents.Any())
            {
                return selectedStudents;
            }

            return new List<Student>();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var selectedStudents = groups.Where(group => group.CourseNumber == courseNumber.CourseNum).SelectMany(i => i.Students);
            if (selectedStudents.Any())
            {
                return new List<Student>(selectedStudents);
            }

            return new List<Student>();
        }

        public Group FindGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new IsuException("Name of group is null or empty");
            }

            if (groupName.Length != 5)
            {
                throw new IsuException("Invalid name to group");
            }

            int courseNumber = 0;
            int groupNumber = 0;

            // FormatException
            if (!int.TryParse(groupName.Substring(2, 1), out courseNumber) || !int.TryParse(groupName.Substring(3, 2), out groupNumber))
            {
                throw new IsuException("Invalid name to group");
            }

            courseNumber = int.Parse(groupName.Substring(2, 1));
            groupNumber = int.Parse(groupName.Substring(3, 2));

            var selectedGroup = groups.Where(group => group.CourseNumber == courseNumber).Where(group => group.GroupNumber == groupNumber).FirstOrDefault();

            if (selectedGroup != null)
            {
                return selectedGroup;
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var selectedGrops = groups.Where(group => group.CourseNumber == courseNumber.CourseNum);
            if (selectedGrops.Any())
            {
                return new List<Group>(selectedGrops);
            }

            return new List<Group>();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (newGroup.Students.Count >= maxStudentInGroup)
            {
                throw new IsuException("Group is full");
            }

            if (groups.Contains(student.StudentGroup))
            {
                student.StudentGroup.Students.Remove(student);
                student.StudentGroup = newGroup;
                newGroup.Students.Add(student);
                return;
            }

            throw new IsuException("Group not changed(error)");
        }
    }
}