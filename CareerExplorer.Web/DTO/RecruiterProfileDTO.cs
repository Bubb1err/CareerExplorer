using CareerExplorer.Core.Entities;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string CompanyDescription { get; set; } 
        public string UserId { get; set; }
    }
}
