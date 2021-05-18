using System;
using System.Collections.Generic;

#nullable disable

namespace Artaplan.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Jobs = new HashSet<Job>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Notes { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
