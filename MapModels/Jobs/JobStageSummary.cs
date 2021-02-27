using Artaplan.MapModels.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.MapModels.Jobs
{
    public class JobStageSummary
    {
        public int JobStageId { get; set; }
        public virtual JobSummary Job { get; set; }
        public virtual StageSummary Stage { get; set; }
    }
}
