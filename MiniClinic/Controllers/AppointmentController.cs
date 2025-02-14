using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniClinic.Entities;
using MiniClinic.Models;

namespace MiniClinic.Controllers
{
    [Authorize(Roles = "Doctor")]  
    public class AppointmentController : Controller
    {
        private readonly MiniClinicDbContext _context;
        private readonly IConfiguration _configuration;

        public AppointmentController(MiniClinicDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult DocumentAppointment(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            var model = new DocumentAppointmentViewModel
            {
                PatientId = patient.Id,
                PatientName = patient.Name,
                Gender = patient.Gender.ToString(), 
                YearOfBirth = patient.DateOfBirth.Year,
                ExternalApiToken = _configuration["ApiToken"],
                MedicalRecord = new MedicalRecordViewClass { PatientId = patient.Id } 
            };

            return View(model);
        }

    }
}
