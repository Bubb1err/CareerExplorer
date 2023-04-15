using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public ICollection<AppUser> Users { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
