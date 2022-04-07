using Microsoft.EntityFrameworkCore;
using Reports.DataAccessLayer.Models;

namespace Reports.DataAccessLayer.Database
{
    public class ReportsContext : DbContext
    {
        public ReportsContext(DbContextOptions<ReportsContext> options) :
            base(options){}
        
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<ReportModel> Reports { get; set; }
    }
}