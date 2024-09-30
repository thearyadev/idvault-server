namespace IdVaultServer.Models
{
    public class DriversLicense : Document
    {
        public string? DriversLicenseNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Class { get; set; }
        public string? Height { get; set; }
        public string? Sex { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
    }

    public class BirthCertificate : Document
    {
        public string? Name { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Birthplace { get; set; }
        public string? DateOfRegistration { get; set; }
        public string? CertificateNumber { get; set; }
        public string? Sex { get; set; }
        public string? RegistrationNumber { get; set; }
    }

    public class Passport : Document
    {
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Nationality { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? Authority { get; set; }
    }
}
