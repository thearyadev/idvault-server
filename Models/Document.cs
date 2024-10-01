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
    
    public class SharedDocument
    {
        public int Id { get; set; }
        [Required]
        public int DocumentId {get; set;}
        [Required]
        public int SenderUserId {get; set;}
        [Required]
        public int ReceiverUserId {get; set;}
        [Required]
        public DateTime ExpiryDate {get; set;}
    }
}
