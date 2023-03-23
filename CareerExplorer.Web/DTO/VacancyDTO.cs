using CareerExplorer.Core.Entities;
using Microsoft.Build.Framework;

namespace CareerExplorer.Web.DTO
{
    public class VacancyDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = false;
        public int CreatorId { get; set; }
        public Recruiter Creator { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsApplied { get; set; }
    }
}
