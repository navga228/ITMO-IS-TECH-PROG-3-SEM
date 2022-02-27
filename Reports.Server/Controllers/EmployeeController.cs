using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.DataAccessLayer.Models;
using Reports.Server.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {

        private IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }


        [HttpGet("{id}")]
        public ActionResult<EmployeeModel> GetById(int id)
        {
            return _service.GetById(id);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteById(int id)
        {
            return _service.DeleteById(id);
        }
        
        [HttpGet]
        public ActionResult<List<EmployeeModel>> Get()
        {
            return _service.GetAll();
        }

        [HttpPost]
        public ActionResult Post([FromForm] EmployeeModel employee)
        {
            return _service.AddEmployee(employee);
        }
           
    }
}