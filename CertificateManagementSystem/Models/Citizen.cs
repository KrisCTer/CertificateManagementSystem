using CertificateManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class Citizen
    {
        [Key]
        [StringLength(20)]
        public string CitizenId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [StringLength(20)]
        public string IdNumber { get; set; }

        [StringLength(100)]
        public string PlaceOfBirth { get; set; }

        [StringLength(200)]
        public string CurrentAddress { get; set; }

        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public byte[] ProfilePicture { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<CertificateRequestz> CertificateRequests { get; set; }
    }
}
