using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniClinic.Entities;
using MiniClinic.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet.Common;
using Newtonsoft.Json.Linq;

namespace MiniClinic.Controllers
{
    public class SymptomController : Controller
    {
        private readonly MiniClinicDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _token;


        public SymptomController(MiniClinicDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _token = _configuration["ApiToken"];
        }
        public IActionResult Index()
        {
            var symptoms = _context.Symptoms.Include(s => s.Patient).ToList();
            return View(symptoms);
        }

        public async Task<IActionResult> Create(int patientId)
        {
            using (var client = new HttpClient())
            {
                var apiUrl = $"https://sandbox-healthservice.priaid.ch/symptoms?token={_token}&format=json&language=en-gb";
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var symptomsList = JsonConvert.DeserializeObject<List<ApiSymptomDto>>(json);
                    ViewBag.Symptoms = new SelectList(symptomsList, "ID", "Name");
                }
                else
                {
                    ViewBag.Symptoms = new SelectList(new List<ApiSymptomDto>(), "ID", "Name");
                    ModelState.AddModelError("", "Unable to retrieve symptoms list from external API.");
                }
            }

            var model = new SymptomViewModel { PatientId = patientId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SymptomViewModel request)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"https://sandbox-healthservice.priaid.ch/symptoms?token={_token}&format=json&language=en-gb";

                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var symptomsList = JsonConvert.DeserializeObject<List<ApiSymptomDto>>(json);
                        var selectedSymptom = symptomsList.FirstOrDefault(s => s.ID == request.SelectedSymptomId);
                        if (selectedSymptom != null)
                        {
                            var patient = _context.Patients.Find(request.PatientId);
                            if (patient == null)
                            {
                                ModelState.AddModelError("", "Patient not found.");
                                return View(request);
                            }

                            var newSymptom = new Symptom
                            {
                                Name = selectedSymptom.Name,
                                PatientId = request.PatientId,
                                Patient = patient
                            };

                            _context.Symptoms.Add(newSymptom);
                            _context.SaveChanges();

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Selected symptom not found in API response.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to retrieve symptoms list from external API.");
                    }
                }
            }

            // If ModelState is invalid, repopulate the dropdown list.
            using (var client = new HttpClient())
            {
                var apiUrl = $"https://sandbox-healthservice.priaid.ch/symptoms?token={_token}&format=json&language=en-gb";
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var symptomsList = JsonConvert.DeserializeObject<List<ApiSymptomDto>>(json);
                    ViewBag.Symptoms = new SelectList(symptomsList, "ID", "Name");
                }
            }
            return View(request);
        }
    }
}
