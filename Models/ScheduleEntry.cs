using System;
using System.Collections.Generic;

#nullable disable

namespace Artaplan.Models
{
    public partial class ScheduleEntry
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int JobStageId { get; set; }
        public int? UserId { get; set; }
        public int ScheduleEntriesId { get; set; }
        public bool? IsDone { get; set; }
        public bool? IsDeadline { get; set; }

        public virtual JobStage JobStage { get; set; }
        public virtual User User { get; set; }
    }
}
