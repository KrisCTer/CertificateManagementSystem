using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitizenshipCertificateandDiplomaManagementSystem.Models
{
    public class EducationalInstitution
    {
        [Key]
        [StringLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string InstitutionId { get; set; }

        [Required]
        [StringLength(200)]
        public string InstitutionName { get; set; }

        [StringLength(50)]
        public string InstitutionType { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Website { get; set; }

        [StringLength(20)]
        public string TaxCode { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FoundingDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Certificate> IssuedCertificates { get; set; }
    }
}
