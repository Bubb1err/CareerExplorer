using Microsoft.Build.Framework;

namespace CareerExplorer.Web.DTO
{
    public class SkillTagDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
