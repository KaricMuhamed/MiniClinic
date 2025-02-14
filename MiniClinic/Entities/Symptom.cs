using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniClinic.Entities
{
    public class Symptom
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
