using System;
using System.Collections.Generic;

#nullable disable

namespace Artaplan.Models
{
    public partial class Slot
    {
        public Slot()
        {
            Jobs = new HashSet<Job>();
            Stages = new HashSet<Stage>();
        }

        public int SlotId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
