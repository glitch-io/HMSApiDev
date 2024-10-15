namespace HMSApi.Models
{
    public class Nurse
    {
        public Guid NurseId { get; set; }
        public string Name { get; set; }
        public string Contactno { get; set; }
        public string Address { get; set; }
        public string MedicalHistory { get; set; }
        public byte[] NurseImg { get; set; }
    }
}
