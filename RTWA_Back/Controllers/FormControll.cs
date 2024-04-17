using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTWA_Back.Data;

namespace RTWA_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class FormController : ControllerBase
    {

        private DataContext _context;

        public FormController(DataContext context)
        {
            //Setting up the datacontext 
            _context = context;
        }


        [HttpGet]
        public IActionResult GetFormByType(string Type)
        {
            //Getting the list of elements for the pop-up component form fields
            try
            {
                var form = _context.FormControlls.Where(e => e.Type == Type).ToList();
                return Ok(form);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
