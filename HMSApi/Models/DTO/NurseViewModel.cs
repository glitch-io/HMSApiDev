namespace HMSApi.Models.DTO
{
    public class NurseViewModel
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Contactno { get; set; }
        public string Address { get; set; }
        public string MedicalHistory { get; set; }
        public string NurseImgBase64 { get; set; }
    }
}
