using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class SystemLog
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        [StringLength(20)]
        public string UserId { get; set; }

        [StringLength(200)]
        public string Action { get; set; }

        public DateTime Timestamp { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        [StringLength(200)]
        public string Device { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
