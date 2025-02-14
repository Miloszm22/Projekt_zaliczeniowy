using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalAppointments.Data;
using Projekt_zaliczeniowy.Models;

namespace Projekt_zaliczeniowy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                DoctorCount = await _context.Doctors.CountAsync(),
                PatientCount = await _context.Patients.CountAsync(),
                AppointmentCount = await _context.Appointments.CountAsync()
            };

            return View(model);
        }
    }
}


