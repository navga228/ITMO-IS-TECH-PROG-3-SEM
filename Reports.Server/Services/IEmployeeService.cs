using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.DataAccessLayer.Models;

namespace Reports.Server.Services
{
    public interface IEmployeeService
    {
        public ActionResult<EmployeeModel> GetById(int id);
        public ActionResult DeleteById(int id);
        public ActionResult<List<EmployeeModel>> GetAll();
        public ActionResult AddEmployee(EmployeeModel employee);
    }
}