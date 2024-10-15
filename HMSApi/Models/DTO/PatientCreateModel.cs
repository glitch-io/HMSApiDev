namespace HMSApi.Models.DTO
{
    public class PatientCreateModel
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string MedicalHistory { get; set; }
        public IFormFile Image { get; set; } 
        public PatientType PatientType { get; set; }
    }
}
