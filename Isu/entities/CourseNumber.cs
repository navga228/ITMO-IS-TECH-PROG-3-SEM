using System.Collections.Generic;
using Isu.Tools;

namespace Isu
{
    public class CourseNumber
    {
        public CourseNumber(int courseNum)
        {
            if (courseNum > 4 || courseNum == 0)
            {
                throw new IsuException("Invalid name to courseNumber");
            }

            CourseNum = courseNum;
        }

        public int CourseNum { get; }
    }
}