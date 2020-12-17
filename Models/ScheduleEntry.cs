using System;
using System.Collections.Generic;

#nullable disable

namespace Artaplan.Models
{
    public partial class ScheduleEntry
    {
        public int ScheduleEntriesId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int JobStageId { get; set; }

        public virtual JobStage JobStage { get; set; }
    }
}
