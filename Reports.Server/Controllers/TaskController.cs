using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.DataAccessLayer.Models;
using Reports.Server.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {

        private ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }


        [HttpGet("{id}")]
        public ActionResult<TaskModel> GetById(int id)
        {
            return _service.GetById(id);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteById(int id)
        {
            return _service.DeleteById(id);
        }

        [HttpGet]
        public ActionResult<List<TaskModel>> Get()
        {
            return _service.GetAll();
        }

        [HttpPost]
        public ActionResult Post([FromForm] int taskId, [FromForm] int assignedEmployee, [FromForm] string description)
        {
            return _service.AddTask(taskId, assignedEmployee, description);
        }

        [HttpPut("update-task/{id}")]
        public ActionResult UpdateTask(int id, [FromForm] string description, [FromForm] TaskModel.StatusType? status)
        {
            if (status != null)
            {
                return _service.UpdateTaskStatus((TaskModel.StatusType) status, id);
            }

            if (!string.IsNullOrEmpty(description))
            {
                return _service.UpdateTaskDescription(description, id);
            }

            return new BadRequestResult();
        }

        [HttpGet("find-by-create/{date}")]
        public ActionResult<List<TaskModel>> GetByCreateDate(string date)
        {
            try
            {
                var dateTime = DateTime.ParseExact(date, "dd.MM.yyyy", null);
                return _service.GetByCreateDate(dateTime);
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        [HttpGet("find-by-update/{date}")]
        public ActionResult<List<TaskModel>> GetByUpdateDate(string date)
        {
            try
            {
                var dateTime = DateTime.ParseExact(date, "dd.MM.yyyy", null);
                return _service.GetByUpdateDate(dateTime);
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        [HttpGet("find-by-head/{head}")]
        public ActionResult<List<TaskModel>> GetByHead(int head)
        {
            try
            {
                return _service.GetByHead(head);
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        [HttpGet("find-by-status/{status}")]
        public ActionResult<List<TaskModel>> GetByStatus(TaskModel.StatusType status)
        {
            try
            {
                return _service.GetByStatus(status);
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}