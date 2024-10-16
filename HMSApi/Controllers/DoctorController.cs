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
    public class DoctorController : ControllerBase
    {
        private readonly HospitalContext _context;

        public DoctorController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorViewModel>>> GetDoctors()
        {
            var doctors = await _context.Set<Doctor>().Include(d => d.Appointments).ToListAsync();

            var doctorViewModels = doctors.Select(doctor => new DoctorViewModel
            {
                id = doctor.DoctorId,
                Name = doctor.Name,
                Contactno = doctor.Contactno,
                Address = doctor.Address,
                MedicalHistory = doctor.MedicalHistory,
                DoctorImgBase64 = doctor.doctorImg != null ? Convert.ToBase64String(doctor.doctorImg) : null
            }).ToList();

            return doctorViewModels;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorViewModel>> GetDoctor(Guid id)
        {
            var doctor = await _context.Set<Doctor>().Include(d => d.Appointments).FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
            {
                return NotFound();
            }

            var doctorViewModel = new DoctorViewModel
            {
                id = doctor.DoctorId,
                Name = doctor.Name,
                Contactno = doctor.Contactno,
                Address = doctor.Address,
                MedicalHistory = doctor.MedicalHistory,
                DoctorImgBase64 = doctor.doctorImg != null ? Convert.ToBase64String(doctor.doctorImg) : null
            };

            return doctorViewModel;
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor([FromForm] DoctorCreateModel model)
        {
            var doctor = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                Name = model.Name,
                Contactno = model.Contactno,
                Address = model.Address,
                MedicalHistory = model.MedicalHistory,
                doctorImg = await ConvertImageToByteArray(model.Image) 
            };

            _context.Set<Doctor>().Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.DoctorId }, doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(Guid id, [FromForm] DoctorCreateModel model)
        {
            var doctor = await _context.Set<Doctor>().FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            doctor.Name = model.Name;
            doctor.Contactno = model.Contactno;
            doctor.Address = model.Address;
            doctor.MedicalHistory = model.MedicalHistory;

            if (model.Image != null)
            {
                doctor.doctorImg = await ConvertImageToByteArray(model.Image);
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var doctor = await _context.Set<Doctor>().FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Set<Doctor>().Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorExists(Guid id)
        {
            return _context.Set<Doctor>().Any(e => e.DoctorId == id);
        }

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
