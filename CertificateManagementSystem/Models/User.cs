using CertificateManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class User
    {
        [Key]
        [StringLength(20)]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(200)]
        public string Organization { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<CertificateRequestz> ProcessedRequests { get; set; }
        public virtual ICollection<Attachmentz> UploadedAttachments { get; set; }
        public virtual ICollection<SystemLog> SystemLogs { get; set; }
        public virtual ICollection<SystemConfiguration> SystemConfigurations { get; set; }
    }
}
