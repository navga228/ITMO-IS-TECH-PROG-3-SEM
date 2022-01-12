using System.Collections.Generic;
using Isu;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Entities.ExtraStudent;
using IsuExtra.Entities.Ognp;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class IsuExtraTests
    {
        private ExtraStudentService _extraStudentService;
        private OgnpService _ognpService;
        [SetUp]
        public void Setup()
        {
            _ognpService = new OgnpService();
            _extraStudentService = new ExtraStudentService();
        }

        [Test]
        public void StudentSignUpforOgnpWithOverlapsSchedule()
        {
            Group group = new Group("M3208");
            Student student = new Student("Вася", group);

            // Создаем расписание для группы студента
            List<Lesson> groupShedule = new List<Lesson>();
            
            // Создаем первый предмет
            string lesson1Name = "Math";
            Time startTimeLesson1 = new Time(14, 30, Time.DaysOfWeek.Monday);
            Time endTimeLesson1 = new Time(16, 0, Time.DaysOfWeek.Monday);
            Lesson lesson1 = new Lesson(lesson1Name, startTimeLesson1, endTimeLesson1, group, "Анна Александровна", "25");
            
            // Создаем второй предмет
            string lesson2Name = "OOP";
            Time startTimeLesson2 = new Time(10, 0, Time.DaysOfWeek.Monday);
            Time endTimeLesson2 = new Time(11, 30, Time.DaysOfWeek.Monday);
            Lesson lesson2 = new Lesson(lesson1Name, startTimeLesson2, endTimeLesson2, group, "Марина Олеговна", "30");
            
            groupShedule.Add(lesson1);
            groupShedule.Add(lesson2);

            // Создаем студента с расписанием группы
            ExtraStudent newExtraStudent = _extraStudentService.AddExtraStudent(student, groupShedule);

            // Создаем расписание для потока огнп
            string lessonPotokName = "Marketing";
            Time startTimeLessonPotok = new Time(10, 0, Time.DaysOfWeek.Monday);
            Time endTimeLessonPotok = new Time(11, 30, Time.DaysOfWeek.Monday);
            Lesson potokLesson = new Lesson(lessonPotokName, startTimeLessonPotok, endTimeLessonPotok, group, "Михаил Андреевич", "30");

            List<Lesson> potokSchedule = new List<Lesson>();
            potokSchedule.Add(potokLesson);

            // Создаем поток
            Potok potok = new Potok("Поток 1", potokSchedule, 50);

            List<Potok> potoks = new List<Potok>();
            potoks.Add(potok);

            // Создаем огнп
            Ognp ognp1 = _ognpService.AddOgnp("Огнп 1", potoks, 'C');
            
            Assert.Catch<OgnpException>(() =>
            {
                _ognpService.SignUpForOgnp(newExtraStudent, ognp1, potok);
            });
        }

        [Test]
        public void SignUpForOgnpStudentHaveOgnpInScheduleAndOgnpPotokHaveStudent()
        {
            Group group = new Group("M3208");
            Student student = new Student("Вася", group);

            List<Lesson> groupShedule = new List<Lesson>();

            string lesson1Name = "Math";
            Time startTimeLesson1 = new Time(14, 30, Time.DaysOfWeek.Monday);
            Time endTimeLesson1 = new Time(16, 0, Time.DaysOfWeek.Monday);
            Lesson lesson1 = new Lesson(lesson1Name, startTimeLesson1, endTimeLesson1, group, "Анна Александровна", "25");
            
            string lesson2Name = "OOP";
            Time startTimeLesson2 = new Time(10, 0, Time.DaysOfWeek.Monday);
            Time endTimeLesson2 = new Time(11, 30, Time.DaysOfWeek.Monday);
            Lesson lesson2 = new Lesson(lesson1Name, startTimeLesson2, endTimeLesson2, group, "Марина Олеговна", "30");
            
            groupShedule.Add(lesson1);
            groupShedule.Add(lesson2);

            ExtraStudent newExtraStudent = _extraStudentService.AddExtraStudent(student, groupShedule);

            string lessonPotokName = "Marketing";
            Time startTimeLessonPotok = new Time(18, 0, Time.DaysOfWeek.Monday);
            Time endTimeLessonPotok = new Time(19, 30, Time.DaysOfWeek.Monday);
            Lesson potokLesson = new Lesson(lessonPotokName, startTimeLessonPotok, endTimeLessonPotok, group, "Михаил Андреевич", "30");

            List<Lesson> potokSchedule = new List<Lesson>();
            potokSchedule.Add(potokLesson);

            Potok potok = new Potok("Поток 1", potokSchedule, 50);

            List<Potok> potoks = new List<Potok>();
            potoks.Add(potok);

            Ognp ognp1 = _ognpService.AddOgnp("Огнп 1", potoks, 'C');
            
            _ognpService.SignUpForOgnp(newExtraStudent, ognp1, potok);
            
            // Проверяем что в расписании студента есть расписание огнп
            Assert.That(newExtraStudent.StudentSchedule.Contains(potokLesson), Is.True);
            
            // Проверяем что студент записан на огнп и состоит в списке потока
            Assert.That(ognp1.Potoks[0].Students.Contains(newExtraStudent), Is.True);
        }

        [Test]
        public void RemoveOgnpAndOgnpRemoved()
        {
            Group group = new Group("M3208");
            Student student = new Student("Вася", group);

            List<Lesson> groupShedule = new List<Lesson>();

            string lesson1Name = "Math";
            Time startTimeLesson1 = new Time(14, 30, Time.DaysOfWeek.Monday);
            Time endTimeLesson1 = new Time(16, 0, Time.DaysOfWeek.Monday);
            Lesson lesson1 = new Lesson(lesson1Name, startTimeLesson1, endTimeLesson1, group, "Анна Александровна", "25");
            
            string lesson2Name = "OOP";
            Time startTimeLesson2 = new Time(10, 0, Time.DaysOfWeek.Monday);
            Time endTimeLesson2 = new Time(11, 30, Time.DaysOfWeek.Monday);
            Lesson lesson2 = new Lesson(lesson1Name, startTimeLesson2, endTimeLesson2, group, "Марина Олеговна", "30");
            
            groupShedule.Add(lesson1);
            groupShedule.Add(lesson2);

            ExtraStudent newExtraStudent = _extraStudentService.AddExtraStudent(student, groupShedule);

            string lessonPotokName = "Marketing";
            Time startTimeLessonPotok = new Time(18, 0, Time.DaysOfWeek.Monday);
            Time endTimeLessonPotok = new Time(19, 30, Time.DaysOfWeek.Monday);
            Lesson potokLesson = new Lesson(lessonPotokName, startTimeLessonPotok, endTimeLessonPotok, group, "Михаил Андреевич", "30");

            List<Lesson> potokSchedule = new List<Lesson>();
            potokSchedule.Add(potokLesson);

            Potok potok = new Potok("Поток 1", potokSchedule, 50);

            List<Potok> potoks = new List<Potok>();
            potoks.Add(potok);

            Ognp ognp1 = _ognpService.AddOgnp("Огнп 1", potoks, 'C');
            
            _ognpService.SignUpForOgnp(newExtraStudent, ognp1, potok);
            
            _ognpService.RemoveOgnp(newExtraStudent, ognp1);
            
            // Проверка на то что в расписании студента нет расписания студента
            Assert.That(newExtraStudent.StudentSchedule.Contains(potokLesson), Is.False);
            // Проверка на то что студент пропал из списка студентов записанных на данное огнп
            Assert.That(ognp1.Potoks[0].Students.Contains(newExtraStudent), Is.False);
        }
    }
}