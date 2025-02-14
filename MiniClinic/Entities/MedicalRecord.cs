using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniClinic.Entities
{
    public class MedicalRecord
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "IDC Description")]
        public string IdcDescription { get; set; }

        [Display(Name = "IDC Code")]
        public string IdcCode { get; set; }
        public int PatientId { get; set; }

        [ForeignKey("PatientId")] 
        public Patient Patient { get; set; }

    }
}
