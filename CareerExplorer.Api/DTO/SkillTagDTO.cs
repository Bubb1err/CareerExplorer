using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Api.DTO
{
    public class SkillTagDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
