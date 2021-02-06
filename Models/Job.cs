using System;
using System.Collections.Generic;

#nullable disable

namespace Artaplan.Models
{
    public partial class Job
    {
        public Job()
        {
            JobStages = new HashSet<JobStage>();
        }

        public int JobId { get; set; }
        public string Status { get; set; }
        public string Tags { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public DateTime? ToStartAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? InsertedAt { get; set; }
        public double Price { get; set; }
        public int? Priority { get; set; }
        public int? EstimatedDays { get; set; }
        public string References { get; set; }
        public int SlotId { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? FinishedAt { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Slot Slot { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<JobStage> JobStages { get; set; }
    }
}
