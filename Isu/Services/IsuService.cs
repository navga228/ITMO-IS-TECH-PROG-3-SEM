using System;
using System.Collections.Generic;
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
                if (i.StudentCounter >= 25)
                {
                    throw new IsuException("The limit of students for the group has been exceeded");
                }

                if (i.CourseNum == group.CourseNum && i.GroupNumber == group.GroupNumber)
                {
                    i.StudentCounter++;
                    Student stu = new Student(name, i);
                    i.Students1.Add(stu);
                    i.StudentCounter++;
                    return stu;
                }

                throw new IsuException("No such group exists");
            }

            throw new IsuException("failed to add student");
        }

        public Student GetStudent(int id)
        {
            foreach (Group i in groups)
            {
                foreach (Student j in i.Students1)
                {
                    if (j.StudID == id)
                    {
                        return j;
                    }
                }
            }

            throw new Exception("No such student exists");
        }

        public Student FindStudent(string name)
        {
            foreach (Group i in groups)
            {
                foreach (Student j in i.Students1)
                {
                    if (j.StudName == name)
                    {
                        return j;
                    }
                }
            }

            throw new Exception("No such student exists");
        }

        public List<Student> FindStudents(string groupName)
        {
            int courseNum = int.Parse(groupName.Substring(2, 1));
            int groupNum = int.Parse(groupName.Substring(3, 2));
            foreach (Group i in groups)
            {
                if (i.CourseNum == courseNum && i.GroupNumber == groupNum)
                {
                    return i.Students1;
                }
            }

            throw new Exception("No such group exists");
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            int curNum = courseNumber.Coursenum;
            List<Student> ans = new List<Student>();
            bool flag = false;
            foreach (Group i in groups)
            {
                if (i.CourseNum == curNum)
                {
                    ans.AddRange(i.Students1);
                    flag = true;
                }
            }

            if (flag) return ans;
            throw new Exception("On this course no students");
        }

        public Group FindGroup(string groupName)
        {
            int courseNum = int.Parse(groupName.Substring(2, 1));
            int groupNum = int.Parse(groupName.Substring(3, 2));
            foreach (Group i in groups)
            {
                if (i.CourseNum == courseNum && i.GroupNumber == groupNum)
                {
                    return i;
                }
            }

            throw new IsuException("No such group exists");
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            int curNum = courseNumber.Coursenum;
            List<Group> ans = new List<Group>();
            bool flag = false;
            foreach (Group i in groups)
            {
                if (i.CourseNum == curNum)
                {
                    ans.Add(i);
                    flag = true;
                }
            }

            if (flag) return ans;
            throw new Exception("On this course no groups");
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            bool groupFound = false;
            bool studFound = false;
            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = 0; j < groups[i].Students1.Count; j++)
                {
                    if (groups[i].Students1[j].StudID == student.StudID)
                    {
                        if (newGroup.StudentCounter >= 25) throw new IsuException("Group is full");
                        groupFound = true;
                        studFound = true;
                        if (groups[i].Students1.Remove(groups[i].Students1[j]))
                        {
                            student.StudGroup = newGroup;
                            newGroup.Students1.Add(student);
                            newGroup.StudentCounter++;
                            groups[i].StudentCounter--;
                        }
                    }
                }

                if (!studFound) throw new IsuException("No such student exists");
            }

            if (!groupFound) throw new Exception("No such group exists");
        }
    }
}