using Artaplan.MapModels.Customers;
using Artaplan.MapModels.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.MapModels.Jobs
{
    public class JobDTO
    {
        public int JobId { get; set; }
        public string Status { get; set; }
        public string Tags { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public DateTime? ToStartAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? InsertedAt { get; set; }
        public double Price { get; set; }
        public int Priority { get; set; }
        public int EstimatedDays { get; set; }
        public string References { get; set; }
        public int SlotId { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public virtual SlotDTO Slot{ get; set; }
        public virtual CustomerDTO Customer { get; set; }
        public virtual ICollection<JobStageDTO> JobStages { get; set; }
    }
}
