using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Api.DTO
{
    public class TokenRequestDTO
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
