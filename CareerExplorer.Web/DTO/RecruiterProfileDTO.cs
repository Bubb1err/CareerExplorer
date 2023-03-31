using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Web.DTO
{
    public class RecruiterProfileDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 
        [Required]
        public string Surname { get; set; } 
        [Required]
        public string Company { get; set; }
        [Required]
        [MinLength(200, ErrorMessage = "Provide at least 200 symbols.")]
        public string CompanyDescription { get; set; } 
        public string UserId { get; set; }
    }
}
