using System;

namespace Reports.DAL.Entities
{
    public class TaskModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int? Employee { get; set; }
        public string Description { get; set; }
        public StatusType Status { get; set; }


        public enum StatusType
        {
            Open,
            Active,
            Resolved
        }
    }
}