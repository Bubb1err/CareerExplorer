
namespace CareerExplorer.Core.Entities
{
    public class Admin
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}