using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class CertificateVerification
    {
        [Key]
        public int VerificationId { get; set; }

        [Required]
        [StringLength(30)]
        public string CertificateId { get; set; }

        public DateTime VerificationDate { get; set; }

        [StringLength(200)]
        public string VerifyingOrganization { get; set; }

        [StringLength(100)]
        public string VerifiedBy { get; set; }

        [StringLength(20)]
        public string VerificationResult { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("CertificateId")]
        public virtual Certificate Certificate { get; set; }
    }
}
