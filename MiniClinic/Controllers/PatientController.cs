using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniClinic.Entities;
using MiniClinic.Models;

namespace MiniClinic.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PatientController : Controller
    {
        private readonly MiniClinicDbContext _context;

        public PatientController(MiniClinicDbContext miniClinicDbContext)
        {
            _context = miniClinicDbContext;
        }

        public IActionResult Index()
        {
            var patients = _context.Patients
                .Include(p => p.Doctor) 
                .ToList();

            return View(patients);
        }

        public IActionResult Create()
        {
            ViewBag.Doctors = _context.Users
                .Where(u => u.Role == UserRole.Doctor)
                .ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(PatientViewModel request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Doctors = _context.Users
                    .Where(u => u.Role == UserRole.Doctor)
                    .ToList();
                return View(request);
            }

            var patient = new Patient
            {
                Name = request.Name,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                DoctorId = request.DoctorId
            };

            try
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();

                ModelState.Clear();
                ViewBag.Message = $"{patient.Name} registered successfully!";
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Please enter valid data!");
                ViewBag.Doctors = _context.Users
                    .Where(u => u.Role == UserRole.Doctor)
                    .ToList();
                return View(request);
            }

            ViewBag.Doctors = _context.Users
                .Where(u => u.Role == UserRole.Doctor)
                .ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
