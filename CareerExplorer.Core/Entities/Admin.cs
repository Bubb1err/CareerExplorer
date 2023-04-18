using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class Admin
    {
        public int Id { get; set; } 
        public virtual ICollection<SkillsTag> Tags { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
