using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HMSApi.Models;
using System.IO;
using System.Threading.Tasks;
using HMSApi.Models.DTO;
using HMSApi.Context;

namespace HMSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly HospitalContext _context;

        public PatientController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Patient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Set<Patient>().Include(p => p.Appointments).ToListAsync();
        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Set<Patient>().Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // POST: api/Patient
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient([FromForm] PatientCreateModel model)
        {
            var patient = new Patient
            {
                Name = model.Name,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                ContactNumber = model.ContactNumber,
                Address = model.Address,
                MedicalHistory = model.MedicalHistory,
                PatientType = model.PatientType,
                Image = await ConvertImageToByteArray(model.Image) // Convert IFormFile to byte[]
            };

            _context.Set<Patient>().Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientId }, patient);
        }

        // PUT: api/Patient/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, [FromForm] PatientCreateModel model)
        {
            var patient = await _context.Set<Patient>().FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            patient.Name = model.Name;
            patient.DateOfBirth = model.DateOfBirth;
            patient.Gender = model.Gender;
            patient.ContactNumber = model.ContactNumber;
            patient.Address = model.Address;
            patient.MedicalHistory = model.MedicalHistory;
            patient.PatientType = model.PatientType;

            if (model.Image != null)
            {
                patient.Image = await ConvertImageToByteArray(model.Image); // Update image if provided
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Patient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Set<Patient>().FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Set<Patient>().Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Set<Patient>().Any(e => e.PatientId == id);
        }

        // Helper method to convert IFormFile to byte[]
        private async Task<byte[]> ConvertImageToByteArray(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }

}
