using System.ComponentModel.DataAnnotations;

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
        public string? Password { get; set; }

        public ICollection<Document>? Documents { get; set;}
    }
}
