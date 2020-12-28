using System;
using System.Collections.Generic;

#nullable disable

namespace Artaplan.Models
{
    public partial class JobStage
    {
        public JobStage()
        {
            ScheduleEntries = new HashSet<ScheduleEntry>();
        }

        public int JobStageId { get; set; }
        public bool IsFinal { get; set; }
        public int WorkHours { get; set; }
        public int JobHours { get; set; }
        public int Order { get; set; }
        public DateTime? Deadline { get; set; }
        public int JobId { get; set; }
        public int StageId { get; set; }

        public virtual Job Job { get; set; }
        public virtual Stage Stage { get; set; }
        public virtual ICollection<ScheduleEntry> ScheduleEntries { get; set; }
    }
}
