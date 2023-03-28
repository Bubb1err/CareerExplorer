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
        public string CreatorNickName { get; set; }
        public string CreatorName { get; set; }
        public string CreatorSurname { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDesciprion { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsApplied { get; set; }
    }
}
