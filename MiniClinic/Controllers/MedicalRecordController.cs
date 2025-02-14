using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniClinic.Entities;
using MiniClinic.Migrations;
using MiniClinic.Models;

namespace MiniClinic.Controllers
{
    public class MedicalRecordController : Controller
    {
        private readonly MiniClinicDbContext _context;

        public MedicalRecordController(MiniClinicDbContext miniClinicDbContext)
        {
            _context = miniClinicDbContext;
        }
        public IActionResult Index(int patientId)
        {
            return View(_context.MedicalRecords.Where(x => x.PatientId == patientId).ToList());
        }

        [HttpPost]
        public IActionResult Create(MedicalRecordViewClass model)
        {
            if (ModelState.IsValid)
            {
               
                    var patient = _context.Patients.Find(model.PatientId);
                    if (patient == null)
                    {
                        ModelState.AddModelError("", "Invalid patient.");
                        return RedirectToAction("Index", new { patientId = model.PatientId });
                    }

                    var newRecord = new MedicalRecord
                    {
                        Name = model.Name,
                        Description = model.Description,
                        IdcDescription = model.IdcDescription,
                        IdcCode = model.IdcCode,
                        PatientId = model.PatientId,
                        Patient = patient 
                    };

                try
                {
                    _context.MedicalRecords.Add(newRecord);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Medical record saved successfully!";
                    return RedirectToAction("Index", new { patientId = model.PatientId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                }
            }

            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            string errorMessageString = "Validation errors: ";
            errorMessageString += string.Join("; ", errorMessages.Where(m => !string.IsNullOrEmpty(m)));

            if (errorMessageString == "Validation errors: ")
            {
                errorMessageString = "Please correct the invalid fields and try again.";
            }

            TempData["ErrorMessage"] = errorMessageString;
            return RedirectToAction("Index", new { patientId = model.PatientId });
        }

    }
}
