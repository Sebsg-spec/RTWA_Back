using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RTWA_Back.Data;
using RTWA_Back.Models;
using ServiceStack.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTWA_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly DataContext _context;

        public LogInController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public LoginModel Login(LoginModel obj)
        {
            try
            {
                var isUserExist = _context.ACCOUNTS.FirstOrDefault(m => m.Email == obj.Email && m.Password == obj.password);

                if (isUserExist != null)
                {
                    obj.Account_Id = isUserExist.Account_Id;
                    obj.Email = isUserExist.Email;
                    obj.Result = true;
                    obj.Message = "Login Success";
                    obj.FullName = isUserExist.FullName;
                }
                else
                {
                    obj.Result = false;
                    obj.Message = "Wrong credentials";
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error from function Login(LoginModel obj): {ex.Message}");
            }
        }

        [HttpPost("Register")] 
        public async Task<ActionResult<List<ACCOUNTS>>> Register(ACCOUNTS obj)
        {
            try
            {
                var isUserExists = await _context.ACCOUNTS
                    .AnyAsync(m => m.Email == obj.Email && m.Password == obj.Password && m.FullName == obj.FullName);

                if (isUserExists)
                {
                    var message = "Account already exists.";
                    return Ok((new { Message = message }));
                }
                else
                {
                    var message = "Account created successfully";
                   
                    _context.ACCOUNTS.Add(obj);
                    await _context.SaveChangesAsync();

                    var newRelation = new RELATIONS
                    {
                        Account_Id = obj.Account_Id.ToString(), 
                        Role_Id = 4
                    };

                    _context.RELATIONS.Add(newRelation);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function CreateSurplusHistory(RequestTablesHistory surplus): {ex.Message}");
            }
        }
    }
}
