using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IdVaultServer.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        [Required]
        [JsonIgnore]
        public int UserId { get; set;}
        [Required]
        [JsonIgnore]
        public User? User { get; set; }
        [Required]
        public string? DocumentType { get; set; }
    }
}
