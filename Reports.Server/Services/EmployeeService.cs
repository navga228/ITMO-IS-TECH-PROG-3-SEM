using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.Server.Contexts;

namespace Reports.Server.Services
{
    public class EmployeeService
    {
        private ReportContext _context;

        public EmployeeService(ReportContext context)
        {
            _context = context;
        }

        public ActionResult<EmployeeModel> GetById(int id)
        {
            try
            {
                return _context.Employees.Single(model => model.Id == id);
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
                _context.Employees.Remove(_context.Employees.Single(model => model.Id == id));
                _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        public ActionResult<List<EmployeeModel>> GetAll()
        {
            try
            {
                return _context.Employees.ToList();
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        public ActionResult AddEmployee(EmployeeModel employee)
        {
            if (employee.Head == null)
            {
                if (_context.Employees.Any())
                {
                    return new BadRequestResult();
                }
            }
            else
            {
                if (_context.Employees.FirstOrDefault(model => model.Id == employee.Head) == null)
                {
                    return new BadRequestResult();
                }
            }

            if (employee.Login == null || employee.Name == null)
            {
                return new BadRequestResult();
            }
            if (_context.Employees.FirstOrDefault(model => employee.Login == model.Login) != null)
            {
                return new BadRequestResult();
            }
            
            _context.Employees.Add(employee);
            _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}