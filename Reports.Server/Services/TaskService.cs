using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reports.DataAccessLayer.Database;
using Reports.DataAccessLayer.Models;

namespace Reports.Server.Services
{
    public class TaskService : ITaskService
    {
        private ReportsContext _context;

        public TaskService(ReportsContext context)
        {
            _context = context;
        }

        public ActionResult<TaskModel> GetById(int id)
        {
            try
            {
                return _context.Tasks.Single(model => model.Id == id);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        public ActionResult DeleteById(int id)
        {
            try
            {
                _context.Tasks.Remove(_context.Tasks.Single(model => model.Id == id));
                _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        public ActionResult<List<TaskModel>> GetAll()
        {
            try
            {
                return _context.Tasks.ToList();
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        public ActionResult AddTask(int taskId, int assignedEmployee, string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return new BadRequestResult();
            }
            TaskModel newTask = new TaskModel(taskId, TaskModel.StatusType.Open, DateTime.Now, DateTime.Now, description, assignedEmployee);
            _context.Tasks.Add(newTask);
            _context.SaveChangesAsync();
            return new OkResult();
        }

        public ActionResult UpdateTaskStatus(TaskModel.StatusType status, int id)
        {
            TaskModel taskModel = _context.Tasks.FirstOrDefault(model => model.Id == id);
            if (taskModel == null) return new BadRequestResult();
            if (taskModel.Status == TaskModel.StatusType.Resolved) return new EmptyResult();
            taskModel.Status = status;
            taskModel.LastUpdateDate = DateTime.Now;
            _context.Update(taskModel);
            _context.SaveChangesAsync();
            return new OkResult();
        }
        
        public ActionResult UpdateTaskDescription(string description, int id)
        {
            TaskModel taskModel = _context.Tasks.FirstOrDefault(model => model.Id == id);
            if (taskModel == null) return new BadRequestResult();
            if (taskModel.Status == TaskModel.StatusType.Resolved) return new EmptyResult();
            taskModel.Description = description;
            taskModel.LastUpdateDate = DateTime.Now;
            _context.Update(taskModel);
            _context.SaveChangesAsync();
            return new OkResult();
        }

        public ActionResult<List<TaskModel>> GetByCreateDate(DateTime dateTime)
        {
            return _context.Tasks.Where(model => model.CreateDate.Date.Equals(dateTime.Date)).ToList();
        }
        
        public ActionResult<List<TaskModel>> GetByUpdateDate(DateTime dateTime)
        {
            return _context.Tasks.Where(model => model.LastUpdateDate.Date.Equals(dateTime.Date)).ToList();
        }

        public ActionResult<List<TaskModel>> GetByStatus(TaskModel.StatusType status, int? employee = null)
        {
            if (employee == null)
            {
                return _context.Tasks.Where(model => model.Status == status).ToList();
            }

            return _context.Tasks.Where(model => model.Status == status && model.AssignedEmployee == employee).ToList();
        }

        public ActionResult<List<TaskModel>> GetByHead(int head)
        {
            var employees = _context.Employees.Where(model => model.Head == head).ToList();
            var tasks = new List<TaskModel>();

            return employees.Aggregate(tasks, (current, employee) => current.Union(_context.Tasks.Where(model => model.AssignedEmployee == employee.Id)).ToList());
        }
    }
}