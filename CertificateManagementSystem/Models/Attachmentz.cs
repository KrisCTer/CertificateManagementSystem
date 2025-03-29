using CitizenshipCertificateandDiplomaManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CertificateManagementSystem.Models
{
    public class Attachmentz
    {
        [Key]
        public int AttachmentId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [StringLength(200)]
        public string DocumentName { get; set; }

        [StringLength(50)]
        public string DocumentType { get; set; }

        [StringLength(200)]
        public string FilePath { get; set; }

        public DateTime UploadDate { get; set; }

        [StringLength(20)]
        public string UploadedBy { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("RequestId")]
        public virtual CertificateRequestz CertificateRequest { get; set; }

        [ForeignKey("UploadedBy")]
        public virtual User Uploader { get; set; }
    }
}
