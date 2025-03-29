using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class Certificate
    {
        [Key]
        [StringLength(30)]
        public string CertificateId { get; set; }

        [Required]
        [StringLength(20)]
        public string CitizenId { get; set; }

        [Required]
        [StringLength(20)]
        public string CertificateTypeId { get; set; }

        [Required]
        [StringLength(20)]
        public string IssuingInstitutionId { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(50)]
        public string Classification { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Score { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(100)]
        public string SignedBy { get; set; }

        [StringLength(100)]
        public string SignerTitle { get; set; }

        [StringLength(200)]
        public string FilePath { get; set; }

        [StringLength(100)]
        public string QRCode { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        [ForeignKey("CitizenId")]
        public virtual Citizen Citizen { get; set; }

        [ForeignKey("CertificateTypeId")]
        public virtual CertificateType CertificateType { get; set; }

        [ForeignKey("IssuingInstitutionId")]
        public virtual EducationalInstitution IssuingInstitution { get; set; }

        public virtual ICollection<CertificateHistory> CertificateHistories { get; set; }
        public virtual ICollection<CertificateVerification> CertificateVerifications { get; set; }
    }
}
