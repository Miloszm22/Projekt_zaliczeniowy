using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalAppointments.Data;

namespace Projekt_zaliczeniowy.Controllers.Api
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/doctors
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Doctors.ToListAsync();
            return Ok(doctors); // Zwracamy listę lekarzy w formacie JSON
        }

        // GET: api/doctors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }
    }
}
