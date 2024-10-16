namespace HMSApi.Models.DTO
{
    public class DoctorViewModel
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Contactno { get; set; }
        public string Address { get; set; }
        public string MedicalHistory { get; set; }
        public string DoctorImgBase64 { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
