using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTWA_Back.Data;
using RTWA_Back.Models;

namespace RTWA_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            //Setting up the datacontext
            _context = context;
        }

        [HttpGet("rolesRequested")]
        public async Task<IActionResult> GetRoleRequests()
        {
            try
            {
                var rolesRequested = _context.RoleUpgrade.ToList();

                if (rolesRequested.Any())
                {
                    //Returning the role requests
                    return Ok(rolesRequested);
                }
                else
                {
                    return Ok(null);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("userRoles")]
        public async Task<IActionResult> GetUserRoles(string accountId)
        {
            //Get the user role based on the account id
            try
            {
                var userRoles = await _context.RELATIONS
                    .Where(r => r.Account_Id == accountId)
                    .Join(_context.ROLES,
                          relation => relation.Role_Id,
                          role => role.Role_Id,
                          (relation, role) => role.role_name)
                    .ToListAsync();

                if (userRoles.Any())
                {
                    //Returning the user role
                    return Ok(userRoles);
                }
                else
                {
                    return NotFound("User roles not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("roleRequest")]
        public async Task<IActionResult> GetUserRoleRequest(string accountId)
        {
            //Get the user role based on the account id
            try
            {
                var userRoleRequest = _context.RoleUpgrade
                    .Where(r => r.Account_id == accountId);

                if (userRoleRequest.Any())
                {
                    //Returning the user role
                    return Ok(userRoleRequest);
                }
                else
                {
                    return Ok(null);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("roleRequest")]
        public async Task<ActionResult<List<RoleUpgrade>>> CreateRoleRequest(RoleUpgrade roleUpgrade)
        {
            try
            {
                _context.RoleUpgrade.Add(roleUpgrade);
                await _context.SaveChangesAsync();
                return Ok(roleUpgrade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function CreateSurplusHistory(RequestTablesHistory package): {ex.Message}");
            }
        }

        [HttpPut("updateRelations/{roleUpgradeId}")]
        public async Task UpdateIdmRelationsAsync(int roleUpgradeId)
        {
            try
            {
                // Retrieve the RoleUpgrade record based on the roleUpgradeId
                var roleUpgrade = await _context.RoleUpgrade
                    .FirstOrDefaultAsync(r => r.Id == roleUpgradeId);

                if (roleUpgrade != null)
                {
                    // Check if RELATIONS record already exists for the account_id
                    var idmRelation = await _context.RELATIONS
                        .FirstOrDefaultAsync(r => r.Account_Id == roleUpgrade.Account_id);

                    if (idmRelation != null)
                    {
                        // Delete the existing record
                        _context.RELATIONS.Remove(idmRelation);

                        // Create a new record with the updated Role_Id
                        idmRelation = new RELATIONS { Account_Id = roleUpgrade.Account_id, Role_Id = roleUpgrade.RoleRequestedId };
                        _context.RELATIONS.Add(idmRelation);
                    }
                    else
                    {
                        // Create new record
                        idmRelation = new RELATIONS { Account_Id = roleUpgrade.Account_id, Role_Id = roleUpgrade.RoleRequestedId };
                        _context.RELATIONS.Add(idmRelation);
                    }
                    _context.RoleUpgrade.Remove(roleUpgrade);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Role upgrade with ID {roleUpgradeId} not found.");
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update RELATIONS table.", ex);
            }
        }


        [HttpDelete("roleDelete/{Id}")]
        public async Task DeleteRoleRequestAsync(int Id)
        {
            try
            {
                var roleRequest = await _context.RoleUpgrade.FindAsync(Id);

                if (roleRequest == null)
                {
                    // Role request not found
                    throw new Exception("Role request not found.");
                }

                _context.RoleUpgrade.Remove(roleRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete role request.", ex);
            }
        }

    }
}
