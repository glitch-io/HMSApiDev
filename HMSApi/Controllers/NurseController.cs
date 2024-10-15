using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HMSApi.Models;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using HMSApi.Models.DTO;
using HMSApi.Context;

namespace HMSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseController : ControllerBase
    {
        private readonly HospitalContext _context;

        public NurseController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Nurse
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NurseViewModel>>> GetNurses()
        {
            var nurses = await _context.Set<Nurse>().ToListAsync();

            // Map the list of Nurse entities to a list of NurseViewModel
            var nurseViewModels = nurses.Select(nurse => new NurseViewModel
            {
                id = nurse.NurseId,
                Name = nurse.Name,
                Contactno = nurse.Contactno,
                Address = nurse.Address,
                MedicalHistory = nurse.MedicalHistory,
                NurseImgBase64 = nurse.NurseImg != null ? Convert.ToBase64String(nurse.NurseImg) : null
            }).ToList();

            return nurseViewModels;
        }


        // GET: api/Nurse/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NurseViewModel>> GetNurse(Guid id)
        {
            var nurse = await _context.Set<Nurse>().FindAsync(id);

            if (nurse == null)
            {
                return NotFound();
            }


            var nurseViewModel = new NurseViewModel
            {
                id = nurse.NurseId,
                Name = nurse.Name,
                Contactno = nurse.Contactno,
                Address = nurse.Address,
                MedicalHistory = nurse.MedicalHistory,
                NurseImgBase64 = nurse.NurseImg != null ? Convert.ToBase64String(nurse.NurseImg) : null 
            };

            return nurseViewModel;
        }

        // GET: api/Nurse/5/image
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetNurseImage(Guid id)
        {
            var nurse = await _context.Set<Nurse>().FindAsync(id);

            if (nurse == null || nurse.NurseImg == null)
            {
                return NotFound();
            }

            // Return the image file as a response
            return File(nurse.NurseImg, "image/jpeg"); // Adjust content type as necessary
        }

        // POST: api/Nurse
        [HttpPost]
        public async Task<ActionResult<Nurse>> PostNurse([FromForm] NurseCreateModel model)
        {
            var nurse = new Nurse
            {
                NurseId = Guid.NewGuid(),
                Name = model.Name,
                Contactno = model.Contactno,
                Address = model.Address,
                MedicalHistory = model.MedicalHistory,
                NurseImg = await ConvertImageToByteArray(model.Image) // Convert IFormFile to byte[]
            };

            _context.Set<Nurse>().Add(nurse);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNurse), new { id = nurse.NurseId }, nurse);
        }

        // PUT: api/Nurse/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNurse(Guid id, [FromForm] NurseCreateModel model)
        {
            var nurse = await _context.Set<Nurse>().FindAsync(id);
            if (nurse == null)
            {
                return NotFound();
            }

            nurse.Name = model.Name;
            nurse.Contactno = model.Contactno;
            nurse.Address = model.Address;
            nurse.MedicalHistory = model.MedicalHistory;

            if (model.Image != null)
            {
                nurse.NurseImg = await ConvertImageToByteArray(model.Image); // Update image if provided
            }

            _context.Entry(nurse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NurseExists(id))
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

        // DELETE: api/Nurse/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNurse(Guid id)
        {
            var nurse = await _context.Set<Nurse>().FindAsync(id);
            if (nurse == null)
            {
                return NotFound();
            }

            _context.Set<Nurse>().Remove(nurse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NurseExists(Guid id)
        {
            return _context.Set<Nurse>().Any(e => e.NurseId == id);
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

    

    // ViewModel to include base64 image

}
