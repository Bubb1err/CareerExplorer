
namespace CareerExplorer.Core.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public ICollection<AppUser> Users { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
