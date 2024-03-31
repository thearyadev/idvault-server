using System.ComponentModel.DataAnnotations;

namespace IdVaultServer.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        [Required]
        public int UserId { get; set;}
        [Required]
        public User? User { get; set; }
        [Required]
        public string? DocumentType { get; set; }
        [Required]
        public string? CreationDate { get; set; }
        [Required]
        public DateTime? ExpirationDate { get; set; }
        [Required]
        public DateTime? IssueDate { get; set; }
        [Required]
        public string? ValidationStatus { get; set; }
    }
}
