using CareerExplorer.Core.Entities;

namespace CareerExplorer.Web.DTO
{
    public class JobSeekerDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? GitHub { get; set; }
        public string? Experience { get; set; }
        public string? UserId { get; set; }
        public virtual ICollection<SkillsTag> Skills { get; set; } = new List<SkillsTag>();
    }
}
