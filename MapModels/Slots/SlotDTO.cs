using Artaplan.MapModels.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.MapModels.Slots
{
    public class SlotDTO
    {
        public int SlotId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<StageDTO> Stages { get; set; }
    }
}
