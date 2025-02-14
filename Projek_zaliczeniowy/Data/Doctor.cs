
namespace MedicalAppointments.Data
{
    public class Doctor
    {
        public int Id { get; set; } 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }= string.Empty;
        public string Specialization { get; set; } = string.Empty;

        // 🔹 Poprawka: Inicjalizacja kolekcji
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}