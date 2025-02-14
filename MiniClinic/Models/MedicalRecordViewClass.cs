namespace MiniClinic.Models
{
    public class MedicalRecordViewClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IdcDescription { get; set; }
        public string IdcCode { get; set; }
        public int PatientId { get; set; }
    }
}
