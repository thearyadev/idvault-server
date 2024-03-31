using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IdVaultServer.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        [JsonIgnore]
        public string? Password { get; set; }
        
        [JsonIgnore]
        public ICollection<Document>? Documents { get; set;}
    }
}
