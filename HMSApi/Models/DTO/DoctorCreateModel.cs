namespace HMSApi.Models.DTO
{
        public class DoctorCreateModel
        {
            public string Name { get; set; }
            public string Contactno { get; set; }
            public string Address { get; set; }
            public string MedicalHistory { get; set; }
            public IFormFile Image { get; set; } 
        }
    }
