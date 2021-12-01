using System;
using Isu;
using IsuExtra.Entities.Ognp;

namespace IsuExtra.Entities
{
    public class Lesson
    {
        private string _teacher;
        private string _class;
        private Group _group;

        public Lesson(string lessonName, Time startTimeLesson, Time endTimeLesson, Group group, string teacher, string cabinet)
        {
            if (startTimeLesson == null)
            {
                throw new OgnpException("Start time lesson is null");
            }

            if (endTimeLesson == null)
            {
                throw new OgnpException("End time lesson is null");
            }

            if (group == null)
            {
                throw new OgnpException("Group is null");
            }

            if (string.IsNullOrEmpty(lessonName))
            {
                throw new OgnpException("Lesson name is null or emty");
            }

            if (string.IsNullOrEmpty(teacher))
            {
                throw new OgnpException("Teacher is null or emty");
            }

            if (string.IsNullOrEmpty(cabinet))
            {
                throw new OgnpException("Cabinet is null or emty");
            }

            StartTimeLesson = startTimeLesson;
            EndTimeLesson = endTimeLesson;
            _group = group;
            _teacher = teacher;
            _class = cabinet;
            LessonName = lessonName;
        }

        public Time StartTimeLesson { get; }
        public Time EndTimeLesson { get; }
        public string LessonName { get; }
    }
}