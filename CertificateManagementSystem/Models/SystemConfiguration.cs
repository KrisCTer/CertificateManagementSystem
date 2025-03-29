using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class SystemConfiguration
    {
        [Key]
        [StringLength(50)]
        public string ConfigId { get; set; }

        [StringLength(100)]
        public string ConfigName { get; set; }

        public string ConfigValue { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(20)]
        public string UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("UpdatedBy")]
        public virtual User User { get; set; }
    }
}
