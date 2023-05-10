using CareerExplorer.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace CareerExplorer.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public UserType UserType { get; set; }
        public int? JobSeekerProfileId { get; set; }
        public JobSeeker JobSeekerProfile { get; set; }
        public int? RecruiterProfileId { get; set; }
        public Recruiter RecruiterProfile { get; set; }
        public int? AdminProfileId { get; set; }
        public Admin AdminProfile { get; set; }
        public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();
    }
}
