using MiniClinic.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniClinic.Models
{
    public class PatientViewModel
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of birth is required!")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public PatientGender Gender { get; set; }

        [Required(ErrorMessage = "Doctor is required!")]
        public int DoctorId { get; set; }
    }
}