using Microsoft.EntityFrameworkCore;
using Reports.DAL.Entities;

namespace Reports.Server.Contexts
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) :
            base(options){}
        
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<ReportModel> Reports { get; set; }
    }
}