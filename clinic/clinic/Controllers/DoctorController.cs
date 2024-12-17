using clinic.models;
using clinic.packpages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IPKG_DOCTOR _pkg;

        public DoctorController(IPKG_DOCTOR pkg)
        {
            _pkg = pkg;
        }
        [HttpGet("/get_doctor")]
        public List<Doctor> Get_doctor()
        {
            List<Doctor> list = new List<Doctor>();
            try
            {
                list = _pkg.Get_doctor();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }
        [HttpGet("/doctor_filter_by_category/{id}")]
        public List<Doctor> GetDoctroBycat(string id)
        {
            List<Doctor> list = new List<Doctor>();

            try
            {
                list = _pkg.Get_doctro_bycat(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }
        [HttpGet("/doctor_filter_by_username/{username}")]
        public List<Doctor> GetDoctroByusername(string username)
        {
            List<Doctor> list = new List<Doctor>();

            try
            {
                list = _pkg.Get_doctro_byusername(username);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }
        [HttpGet("/doctor/by_id/{id}")]
        public async Task<IActionResult> GetDoctorById(string id)
        {
            var doctor = _pkg.GetDoctorById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }
        [HttpGet("/doctor_filter_by_category_name/{category_name}")]
        public List<Doctor> GetDoctroBycategoryName(string category_name)
        {
            List<Doctor> list = new List<Doctor>();

            try
            {
                list = _pkg.Get_doctro_by_category_name(category_name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }
        [HttpDelete("/doctor/delete/by_id/{id}")]
        [Authorize(Policy = "AdminOnly")] 

        public IActionResult DeleteDoctorById(int id)
        {
            try
            {
                _pkg.Delete_doctor_by_id(id);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return Ok("doctor deleted successfully");

        }
    }
}
