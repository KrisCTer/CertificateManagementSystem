using CertificateManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class CertificateType
    {
        [Key]
        [StringLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string CertificateTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string CertificateTypeName { get; set; }

        [StringLength(50)]
        public string Level { get; set; }

        [StringLength(100)]
        public string FieldOfStudy { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int? ValidityPeriod { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<CertificateRequestz> CertificateRequests { get; set; }
    }
}
