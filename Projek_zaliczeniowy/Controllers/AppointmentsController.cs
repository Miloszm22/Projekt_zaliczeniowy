using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalAppointments.Data;
using System.Linq;
using System.Threading.Tasks;

[Authorize] // Każdy zalogowany użytkownik może wejść
public class AppointmentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AppointmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            ViewBag.ErrorMessage = "Musisz się zalogować, aby zobaczyć listę wizyt.";
            return View(new List<Appointment>());
        }

        var user = await _userManager.GetUserAsync(User);
        bool isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

        ViewBag.IsAdmin = isAdmin;

        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ToListAsync();

        return View(appointments);
    }


    [Authorize(Roles = "Admin")] 
    public IActionResult Create()
    {
        ViewBag.Patients = _context.Patients
            .Select(p => new { p.Id, FullName = p.FirstName + " " + p.LastName })
            .ToList();

        ViewBag.Doctors = _context.Doctors
            .Select(d => new { d.Id, FullName = d.FirstName + " " + d.LastName })
            .ToList();

        return View();
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Patients = _context.Patients
            .Select(p => new { p.Id, FullName = p.FirstName + " " + p.LastName })
            .ToList();

        ViewBag.Doctors = _context.Doctors
            .Select(d => new { d.Id, FullName = d.FirstName + " " + d.LastName })
            .ToList();

        return View(appointment);
    }

    [Authorize(Roles = "Admin")] // ⬅ Tylko Admin może usuwać wizyty
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (appointment == null) return NotFound();

        return View(appointment);
    }


    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")] //Tylko Admin może usuwać wizyty
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

}

