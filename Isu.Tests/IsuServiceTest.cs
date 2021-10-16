using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group testGroup = _isuService.AddGroup("M3208");
            Student teststudent = _isuService.AddStudent(testGroup, "Петр");
            Assert.That(teststudent.StudentGroup, Is.SameAs(testGroup));
            Assert.That(_isuService.FindStudents(testGroup.GroupName).Contains(teststudent), Is.True);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Group testGroup = _isuService.AddGroup("M3208");
            Assert.Catch<IsuException>(() =>
            {
                for (int i = 0; i < 26; i++)
                {
                    Student testStudent = _isuService.AddStudent(testGroup, i.ToString());
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {

            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("M320800");
                _isuService.AddGroup("Вацок");
                _isuService.AddGroup("M3908");
                _isuService.AddGroup("M1208");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {

            Group newGroup = _isuService.AddGroup("M3208");
            Group oldGroup = _isuService.AddGroup("M3206");
            Student testStudent = _isuService.AddStudent(oldGroup, "Гога");
            _isuService.ChangeStudentGroup(testStudent, newGroup);
            Assert.That(testStudent.StudentGroup, Is.SameAs(newGroup));
        }
    }
}