using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTWA_Back.Data;
using RTWA_Back.Models;


namespace RTWA_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageDetailsController : ControllerBase
    {
        private DataContext _context;

        public PackageDetailsController(DataContext context)
        {
            //Setting up the datacontext
            _context = context;
        }

        //Function to get the datils of the package based on the requestTableId
        [HttpGet("GetDataByPackageId/{packageId}")]
        public IActionResult GetDataByPackageId(Guid PackageId)
        {
            try
            {
                var data = _context.PackageDetails.Where(e => e.PackageId == PackageId).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function GetDataByRequestTableId(Guid requestTableId): {ex.Message}");
            }
        }

        //Function to create the data of the package in the RequestTableDetailsHistory table
        [HttpPost]
        public async Task<ActionResult<List<PackageDetails>>> CreatePackageDetails(PackageDetails package)
        {
            try
            {
                _context.PackageDetails.Add(package);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function CreateSurplus( RequestTableDetails package ): {ex.Message}");
            }


        }

        //Function for moving the details of a package from the main table to the history table to keep track of the data
        [HttpPost("CreatePackageDetailsHistory")]
        public async Task<ActionResult<List<PackageDetailsHistory>>> CreatePackageDetailsHistory(PackageDetailsHistory package)
        {
            try
            {
                _context.PackageDetailsHistory.Add(package);
                await _context.SaveChangesAsync();
                return Ok(await _context.PackageDetailsHistory.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function CreateDetailsHistory(RequestTablesDetailsHistory package): {ex.Message}");
            }

        }

        //Function for deleting the package details after a person is selected
        [HttpDelete("{Id}")]
        public async Task<ActionResult<List<PackageDetails>>> DeletePackageDetails(int Id)
        {
            try
            {
                var dbTable = await _context.PackageDetails.FindAsync(Id);

                if (dbTable == null)
                    return BadRequest("Not Found");

                _context.PackageDetails.Remove(dbTable);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from funcion DeleteSurplus(int Id): {ex.Message}");
            }

        }

    }
}
