using CitizenshipCertificateandDiplomaManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CertificateManagementSystem.Models
{
    public class CertificateRequestz
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        [StringLength(20)]
        public string CitizenId { get; set; }

        [Required]
        [StringLength(20)]
        public string CertificateTypeId { get; set; }

        [StringLength(50)]
        public string RequestType { get; set; }

        public DateTime SubmissionDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpectedCompletionDate { get; set; }

        [StringLength(20)]
        public string ProcessedBy { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("CitizenId")]
        public virtual Citizen Citizen { get; set; }

        [ForeignKey("CertificateTypeId")]
        public virtual CertificateType CertificateType { get; set; }

        [ForeignKey("ProcessedBy")]
        public virtual User Processor { get; set; }

        public virtual ICollection<Attachmentz> Attachments { get; set; }
    }
}
