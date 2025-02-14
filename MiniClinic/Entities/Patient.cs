using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace MiniClinic.Entities
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of birth is required!")]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public PatientGender Gender { get; set; }

        [Required(ErrorMessage = "Doctor is required!")]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public User Doctor { get; set; }

    }

    public enum PatientGender
    {
        Male,
        Female
    }
}
