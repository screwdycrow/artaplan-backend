using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.MapModels.Jobs
{
    public class ScheduleEntryDTO
    {
        public int? UserId { get; set; }
        public int ScheduleEntriesId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int JobStageId { get; set; } 

    }
}
