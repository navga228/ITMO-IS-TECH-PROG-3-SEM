using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Reports.DataAccessLayer.Models;

namespace Reports.Server.Services
{
    public interface IReportService
    {
        public ActionResult<ReportModel> GetById(int id);
        public ActionResult DeleteById(int id);
        public ActionResult<List<ReportModel>> GetAll();
        public ActionResult<List<TaskModel>> GetWeekTask();
        public ActionResult AddWeeklyResult(int employee);
        public ActionResult AddTaskToReport(int reportId, int taskId);
        public ActionResult ChangeStatus(int id, ReportModel.ReportStatus status);
        public ActionResult<List<ReportModel>> GetByHead(int head);
    }
}