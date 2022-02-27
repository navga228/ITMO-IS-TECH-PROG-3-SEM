using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reports.DataAccessLayer.Database;
using Reports.DataAccessLayer.Models;

namespace Reports.Server.Services
{
    public class ReportService : IReportService
    {
        private ReportsContext _context;

        public ReportService(ReportsContext context)
        {
            _context = context;
        }

        public ActionResult<ReportModel> GetById(int id)
        {
            try
            {
                return _context.Reports.Single(model => model.Id == id);
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
                _context.Reports.Remove(_context.Reports.Single(model => model.Id == id));
                _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        public ActionResult<List<ReportModel>> GetAll()
        {
            try
            {
                return _context.Reports.ToList();
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        public ActionResult<List<TaskModel>> GetWeekTask()
        {
            try
            {
                return _context.Tasks.Where(model => (DateTime.Now - model.CreateDate).Days < 7).ToList();
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        public ActionResult AddWeeklyResult(int employee)
        {
            List<TaskModel> taskModels = GetWeekTask().Value;
            if (taskModels != null)
            {
                List<TaskModel> tasks =
                    taskModels.FindAll(model => model.Status == TaskModel.StatusType.Resolved && model.AssignedEmployee == employee);
            }
            else
            {
                taskModels = new List<TaskModel>();
            }

            var report = new ReportModel
            {
                Status = ReportModel.ReportStatus.Active,
                Tasks = new List<TaskModel>(taskModels),
                Employee = employee
            };
            _context.Reports.Add(report);
            _context.SaveChangesAsync();
            return new OkResult();
        }

        public ActionResult AddTaskToReport(int reportId, int taskId)
        {
            try
            {
                ReportModel report = _context.Reports.FirstOrDefault(model => model.Id == reportId);
                TaskModel task = _context.Tasks.FirstOrDefault(model => model.Id == taskId);
                if (report == null || task == null) return new BadRequestResult();
                if (report.Status == ReportModel.ReportStatus.Close) return new BadRequestResult();
                report.Tasks.Add(task);
                _context.SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        public ActionResult ChangeStatus(int id, ReportModel.ReportStatus status)
        {
            try
            {
                ReportModel report = _context.Reports.FirstOrDefault(model => model.Id == id);
                if (report == null) return new BadRequestResult();
                report.Status = status;
                _context.SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        public ActionResult<List<ReportModel>> GetByHead(int head)
        {
            var employees = _context.Employees.Where(model => model.Head == head).ToList();
            var reports = new List<ReportModel>();

            return employees.Aggregate(reports, (current, employee) => current.Union(_context.Reports.Where(model => model.Employee == employee.Id)).ToList());
        }
    }
}