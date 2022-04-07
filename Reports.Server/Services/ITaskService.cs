using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.DataAccessLayer.Models;

namespace Reports.Server.Services
{
    public interface ITaskService
    {
        public ActionResult<TaskModel> GetById(int id);
        public ActionResult DeleteById(int id);
        public ActionResult<List<TaskModel>> GetAll();
        public ActionResult AddTask(int taskId, int AssignedEmployee, string Description);
        public ActionResult UpdateTaskStatus(TaskModel.StatusType status, int id);
        public ActionResult UpdateTaskDescription(string description, int id);
        public ActionResult<List<TaskModel>> GetByCreateDate(DateTime dateTime);
        public ActionResult<List<TaskModel>> GetByUpdateDate(DateTime dateTime);
        public ActionResult<List<TaskModel>> GetByStatus(TaskModel.StatusType status, int? employee = null);
        public ActionResult<List<TaskModel>> GetByHead(int head);
    }
}