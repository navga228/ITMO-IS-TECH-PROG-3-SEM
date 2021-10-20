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
            foreach (Group i in groups)
            {
                if (i.Students.Count >= maxStudentInGroup)
                {
                    throw new IsuException("The limit of students for the group has been exceeded");
                }

                if (i.CourseNumber == group.CourseNumber && i.GroupNumber == group.GroupNumber)
                {
                    Student stu = new Student(name, i);
                    i.Students.Add(stu);
                    return stu;
                }
            }

            throw new IsuException("No such group exists");
        }

        public Student GetStudent(int id)
        {
            foreach (Student j in groups.SelectMany(i => i.Students.Where(j => j.StudentId == id)))
            {
                return j;
            }

            throw new IsuException("No such student exist");
        }

        public Student FindStudent(string name)
        {
            foreach (Student student in groups.SelectMany(i => i.Students.Where(student => student.StudentName == name)))
            {
                return student;
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
            if (!int.TryParse(groupName.Substring(2, 1), out courseNumber) || !int.TryParse(groupName.Substring(3, 2), out groupNumber))
            {
                throw new IsuException("Invalid name to group");
            }

            courseNumber = int.Parse(groupName.Substring(2, 1));
            groupNumber = int.Parse(groupName.Substring(3, 2));
            foreach (Group group in groups.Where(group => group.GroupNumber == groupNumber).Where(group => group.CourseNumber == courseNumber))
            {
                return group.Students;
            }

            return new List<Student>();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            List<Student> ans = new List<Student>();
            foreach (Group group in groups.Where(group => group.CourseNumber == courseNumber.CourseNum))
            {
                ans.AddRange(group.Students);
            }

            if (ans.Count > 0)
            {
                return ans;
            }

            return new List<Student>();
        }

        public Group FindGroup(string groupName)
        {
            int courseNumber = int.Parse(groupName.Substring(2, 1));
            int groupNumber = int.Parse(groupName.Substring(3, 2));

            foreach (Group group in groups.Where(group => group.CourseNumber == courseNumber).Where(group => group.GroupNumber == groupNumber))
            {
                return group;
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            List<Group> ans = new List<Group>();
            foreach (Group group in groups.Where(group => group.CourseNumber == courseNumber.CourseNum))
            {
                ans.Add(group);
            }

            if (ans.Count > 0)
            {
                return ans;
            }

            return new List<Group>();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (newGroup.Students.Count >= maxStudentInGroup)
            {
                throw new IsuException("Group is full");
            }

            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = 0; j < groups[i].Students.Count; j++)
                {
                    if (groups[i].Students[j].StudentId == student.StudentId)
                    {
                        groups[i].Students.Remove(groups[i].Students[j]);
                        student.StudentGroup = newGroup;
                        newGroup.Students.Add(student);
                        return;
                    }
                }
            }

            throw new IsuException("Group not changed(error)");
        }
    }
}