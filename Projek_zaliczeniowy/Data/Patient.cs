namespace MedicalAppointments.Data
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
// zmiany, dodane string.empty