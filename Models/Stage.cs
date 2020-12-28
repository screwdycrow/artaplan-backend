using System;
using System.Collections.Generic;

#nullable disable

namespace Artaplan.Models
{
    public partial class Stage
    {
        public Stage()
        {
            JobStages = new HashSet<JobStage>();
        }

        public int StageId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int EstimatedHours { get; set; }
        public double? AvgHours { get; set; }
        public string Tags { get; set; }
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public int Order { get; set; }
        public virtual Slot Slot { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<JobStage> JobStages { get; set; }
    }
}
