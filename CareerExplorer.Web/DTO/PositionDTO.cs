using Microsoft.Build.Framework;

namespace CareerExplorer.Web.DTO
{
    public class PositionDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
