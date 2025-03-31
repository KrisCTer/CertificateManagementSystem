using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class SystemLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int LogId { get; set; }

        [StringLength(450)]  // Ensure the length matches the length of AspNetUsers.Id
        public string UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Action { get; set; }

        public DateTime Timestamp { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        [StringLength(200)]
        public string Device { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}
