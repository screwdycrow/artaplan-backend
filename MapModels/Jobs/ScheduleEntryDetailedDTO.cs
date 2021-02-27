using Artaplan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.MapModels.Jobs
{
    public class ScheduleEntryDetailedDTO
    {
        public int ScheduleEntriesId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int JobStageId { get; set; }
        public int UserId { set; get; }
        public virtual JobStageSummary JobStage { get; set; }

    }
}
