using System;
using System.Collections.Generic;

namespace GamesCatalog.DAL.Models
{
    public partial class Developer
    {
        public Developer()
        {
            Games = new HashSet<Game>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; }
    }
}
