using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.MapModels.Jobs
{
    public class JobStageDTO
    {
        public int JobStageId { get; set; }
        public bool IsFinal { get; set; }
        public int WorkHours { get; set; }
        public int JobHours { get; set; }
        public int Order { get; set; }
        public DateTime? Deadline { get; set; }
        public int JobId { get; set; }
        public int StageId { get; set; }

    }
}
