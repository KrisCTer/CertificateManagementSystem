using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class SystemConfiguration
    {
        [Key]
        [StringLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ConfigId { get; set; }

        [Required]
        [StringLength(100)]
        public string ConfigName { get; set; }

        [Required]
        public string ConfigValue { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        public virtual User UpdatedByUser { get; set; }
    }

}
