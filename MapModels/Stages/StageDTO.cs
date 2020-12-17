using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.MapModels.Stages
{
    public class StageDTO
    {
        public int StageId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int EstimatedHours { get; set; }
        public double? AvgHours { get; set; }
        public string Tags { get; set; }
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public int Order { get; set; }


    }
}
