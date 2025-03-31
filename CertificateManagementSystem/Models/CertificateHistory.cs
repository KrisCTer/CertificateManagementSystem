using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class CertificateHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int HistoryId { get; set; }

        [Required]
        [StringLength(36)]
        public string CertificateId { get; set; }

        [StringLength(20)]
        public string PreviousStatus { get; set; }

        [StringLength(20)]
        public string NewStatus { get; set; }

        public DateTime ChangeDate { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(100)]
        public string ChangedBy { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("CertificateId")]
        public virtual Certificate Certificate { get; set; }
    }
}
