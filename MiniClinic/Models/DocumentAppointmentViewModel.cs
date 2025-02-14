using MiniClinic.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniClinic.Models
{
    public class DocumentAppointmentViewModel
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }  
        public int YearOfBirth { get; set; }
        public int SelectedDiagnosisId { get; set; }
        public string ExternalApiToken { get; set; }
        public MedicalRecordViewClass MedicalRecord { get; set; }
    }
}
