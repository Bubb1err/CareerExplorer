using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class JobSeeker
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string GitHub { get; set; }
        public string Experience { get; set; }
        public int Views { get; set; }
        public int UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
