using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.DataAccessLayer.Models;
using Reports.Server.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {

        private IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }


        [HttpGet("{id}")]
        public ActionResult<ReportModel> GetById(int id)
        {
            return _service.GetById(id);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteById(int id)
        {
            return _service.DeleteById(id);
        }
        
        [HttpGet]
        public ActionResult<List<ReportModel>> Get()
        {
            return _service.GetAll();
        }

        [HttpPost]
        public ActionResult CreateWeeklyReport([FromForm]int employee)
        {
            return _service.AddWeeklyResult(employee);
        }

        [HttpGet("week-tasks")]
        public ActionResult<List<TaskModel>> GetWeekTasks()
        {
            return _service.GetWeekTask();
        }

        [HttpPut("add-task")]
        public ActionResult AddTaskToReport([FromForm] int reportId, [FromForm] int taskId)
        {
            return _service.AddTaskToReport(reportId, taskId);
        }
        
        [HttpPut("status/{id}")]
        public ActionResult AddTaskToReport(int id, [FromForm] ReportModel.ReportStatus status)
        {
            return _service.ChangeStatus(id, status);
        }
        
        [HttpGet("find-by-head/{head}")]
        public ActionResult<List<ReportModel>> GetByHead(int head)
        {
            return _service.GetByHead(head);
        }

    }
}