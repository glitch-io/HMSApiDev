namespace HMSApi.Models
{

        public class Patient
        {      
        public int PatientId { get; set; }
        public string Name { get; set; }
       
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string MedicalHistory { get; set; }
        public byte[] Image { get; set; }
        public PatientType PatientType { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        }
        public enum PatientType
        {
            Inpatient,
            Outpatient
        }

}

