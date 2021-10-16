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
                if (i.Students.Count >= 25)
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

            throw new IsuException("No such student exists");
        }

        public Student FindStudent(string name)
        {
            foreach (Student student in groups.SelectMany(i => i.Students.Where(student => student.StudentName == name)))
            {
                return student;
            }

            throw new IsuException("No such student exists");
        }

        public List<Student> FindStudents(string groupName)
        {
            int courseNumber = int.Parse(groupName.Substring(2, 1));
            int groupNumber = int.Parse(groupName.Substring(3, 2));
            foreach (Group group in groups.Where(group => group.GroupNumber == groupNumber).Where(group => group.CourseNumber == courseNumber))
            {
                return group.Students;
            }

            throw new IsuException("No such group exists");
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

            throw new IsuException("On this course no students");
        }

        public Group FindGroup(string groupName)
        {
            int courseNumber = int.Parse(groupName.Substring(2, 1));
            int groupNumber = int.Parse(groupName.Substring(3, 2));

            foreach (Group group in groups.Where(group => group.CourseNumber == courseNumber).Where(group => group.GroupNumber == groupNumber))
            {
                return group;
            }

            throw new IsuException("No such group exists");
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

            throw new IsuException("On this course no groups");
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (newGroup.Students.Count >= 25)
            {
                throw new IsuException("Group is full");
            }

            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = 0; j < groups[i].Students.Count; j++)
                {
                    if (groups[i].Students[j].StudentId == student.StudentId)
                    {
                        if (groups[i].Students.Remove(groups[i].Students[j]))
                        {
                            student.StudentGroup = newGroup;
                            newGroup.Students.Add(student);
                            return;
                        }
                    }
                }
            }

            throw new IsuException("Group not changed(error)");
        }
    }
}