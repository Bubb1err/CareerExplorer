using CareerExplorer.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Web.DTO
{
    public class CreateOrEditVacancyDTO
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        [Required]
        [MinLength(200, ErrorMessage = "Provide at least 200 symbols.")]
        public string Description { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
