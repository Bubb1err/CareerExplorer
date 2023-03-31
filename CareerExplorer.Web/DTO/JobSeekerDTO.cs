using CareerExplorer.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerExplorer.Web.DTO
{
    public class JobSeekerDTO
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        [Phone]
        public string? Phone { get; set; }
        public string? GitHub { get; set; }
        [Required]
        [MinLength(100, ErrorMessage =  "Experience must contain at least 100 symbols.")]
        public string? Experience { get; set; }
        public string? UserId { get; set; }
        public virtual ICollection<SkillsTag> Skills { get; set; } = new List<SkillsTag>();
    }
}
