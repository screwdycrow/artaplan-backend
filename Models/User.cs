using System;
using System.Collections.Generic;

#nullable disable

namespace Artaplan.Models
{
    public partial class User
    {
        public User()
        {
            Customers = new HashSet<Customer>();
            Jobs = new HashSet<Job>();
            ScheduleEntries = new HashSet<ScheduleEntry>();
            Slots = new HashSet<Slot>();
            Stages = new HashSet<Stage>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<ScheduleEntry> ScheduleEntries { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
    }
}
