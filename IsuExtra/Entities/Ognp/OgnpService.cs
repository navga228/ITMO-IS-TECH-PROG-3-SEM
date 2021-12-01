using System;
using System.Collections.Generic;
using System.Linq;
using Isu;
using IsuExtra.Entities.Ognp;

namespace IsuExtra.Entities.Ognp
{
    public class OgnpService
    {
        public Ognp AddOgnp(string courseName, List<Potok> potoks, char megaFacultyLetter)
        {
            Ognp newOgnp = new Ognp(courseName, potoks, megaFacultyLetter);
            return newOgnp;
        }

        public void SignUpForOgnp(ExtraStudent.ExtraStudent extraStudent, Ognp ognp, Potok potok)
        {
            if (extraStudent == null)
            {
                throw new OgnpException("Extra student is null");
            }

            if (ognp == null)
            {
                throw new OgnpException("Ognp is null");
            }

            if (potok == null)
            {
                throw new OgnpException("Potok is null");
            }

            if (extraStudent.Student.StudentGroup.DerectionLetter == ognp.MegaFacultyLetter)
            {
                throw new OgnpException("Can't sign up for ognp your faculty");
            }

            if (extraStudent.Ognps.Count >= 2)
            {
                throw new OgnpException("Can sign up only for 2 ognp");
            }

            if (potok.Students.Count >= potok.MaxStudentInPotok)
            {
                throw new OgnpException("Potok is full");
            }

            foreach (var studentLesson in extraStudent.StudentSchedule)
            {
                foreach (var potokLesson in potok.PotokSchedule)
                {
                    int endTimeStudentLessonInMinutes = (studentLesson.EndTimeLesson.Hour * 60) + studentLesson.EndTimeLesson.Minutes;
                    int endTimePototkLessonInMinutes = (potokLesson.EndTimeLesson.Hour * 60) + potokLesson.EndTimeLesson.Minutes;
                    if (Math.Abs(endTimeStudentLessonInMinutes - endTimePototkLessonInMinutes) <= 90)
                    {
                        throw new OgnpException("Schedule intersect");
                    }
                }
            }

            foreach (var potokLesson in potok.PotokSchedule)
            {
                extraStudent.StudentSchedule.Add(potokLesson);
            }

            extraStudent.Ognps.Add(ognp);
            potok.Students.Add(extraStudent);
        }

        public void RemoveOgnp(ExtraStudent.ExtraStudent extraStudent, Ognp ognp)
        {
            if (extraStudent == null)
            {
                throw new OgnpException("Extra Student is null");
            }

            if (ognp == null)
            {
                throw new OgnpException("Ognp is null");
            }

            if (!extraStudent.Ognps.Contains(ognp))
            {
                throw new OgnpException("Student don't have this ognp");
            }

            var potok = ognp.Potoks.Where(potok => potok.Students.Contains(extraStudent)).FirstOrDefault();
            foreach (var lesson in potok.PotokSchedule)
            {
                extraStudent.StudentSchedule.Remove(lesson);
            }

            potok.Students.Remove(extraStudent);
            extraStudent.Ognps.Remove(ognp);
        }

        public List<Potok> GetPotoks(Ognp ognp)
        {
            if (ognp == null)
            {
                throw new OgnpException("Ognp is null");
            }

            return ognp.Potoks;
        }

        public List<ExtraStudent.ExtraStudent> GetStudents(Ognp ognp, Potok potok)
        {
            if (ognp == null)
            {
                throw new OgnpException("Ognp is null");
            }

            if (potok == null)
            {
                throw new OgnpException("Ognp is null");
            }

            var potokAnsver = ognp.Potoks.Where(ognpPotok => ognpPotok.PotokName == potok.PotokName).FirstOrDefault();
            return potokAnsver.Students;
        }
    }
}