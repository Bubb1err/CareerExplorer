using CareerExplorer.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Web.DTO
{
    public class JobSeekerViewProfileDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? GitHub { get; set; }
        public string? Experience { get; set; }
        public string? NickName { get; set; }
        public bool IsAccepted { get; set; }

    }
}
