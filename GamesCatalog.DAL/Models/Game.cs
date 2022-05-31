using System;
using System.Collections.Generic;

namespace GamesCatalog.DAL.Models
{
    public partial class Game
    {
        public Game()
        {
            Genres = new HashSet<Genre>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid DeveloperId { get; set; }

        public virtual Developer Developer { get; set; } = null!;

        public virtual ICollection<Genre> Genres { get; set; }
    }
}
