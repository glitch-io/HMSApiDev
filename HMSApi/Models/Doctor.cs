namespace HMSApi.Models
{
    public class Doctor
    {
        public Guid DoctorId { get; set; }
        public string Name { get; set; }
        public string Contactno { get; set; }
        public string Address { get; set; }
        public string MedicalHistory { get; set; }
        public byte[] doctorImg { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

    }
}
