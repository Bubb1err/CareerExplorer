using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Web.DTO
{
    public class CreateOrEditVacancyDTO
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
