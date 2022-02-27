using System;

namespace Reports.DataAccessLayer.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int? AssignedEmployee { get; set; }
        public string Description { get; set; }
        public StatusType Status { get; set; }

        public TaskModel(int id, StatusType status, DateTime createDate, DateTime lastUpdateDate, string description, int assignedEmployee)
        {
            Id = id;
            Status = status;
            CreateDate = createDate;
            LastUpdateDate = lastUpdateDate;
            Description = description;
            AssignedEmployee = assignedEmployee;
        }
        public TaskModel()
        {}

        public enum StatusType
        {
            Open,
            Active,
            Resolved
        }
    }
}