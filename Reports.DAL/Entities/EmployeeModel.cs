using System;

namespace Reports.DAL.Entities
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Head { get; set; }
        public string Login { get; set; }
    }
}