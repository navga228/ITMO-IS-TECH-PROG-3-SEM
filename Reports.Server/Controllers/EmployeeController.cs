using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.Server.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {

        private EmployeeService _service;

        public EmployeeController(EmployeeService service)
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