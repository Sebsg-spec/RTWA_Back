using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTWA_Back.Data;
using RTWA_Back.Models;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RTWA_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private DataContext _context;
        private readonly IConfiguration _config;

        public PackageController(DataContext context, IConfiguration config)
        {
            //Setting up the datacontext
            _context = context;
            _config = config;
        }

        //Function to get the main tables on the demand and offer pages 
        [HttpGet("GetDataByType/{type}")]
        public IActionResult GetDataByType(int type)
        {
            try
            {
                var data = _context.Package.Where(e => e.Type == type).ToList();

                return Ok(data);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function GetDataByType(int type) : {ex.Message}");
            }
        }

        //Function to get a certain package based on the PackageUID 
        [HttpGet("GetDataByPackageUID/{packageUID}")]
        public IActionResult GetDataByPackageUID(Guid packageUID)
        {
            try
            {
                var data = _context.Package.Where(e => e.PackageUID == packageUID).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function GetDataByPackageUID(Guid PackageUID): {ex.Message}");
            }
        }

        //Function to get the latest row created by user
        [HttpGet("GetDataByLatestAndUser/{user}")]
        public IActionResult GetDataByLatestAndUser(string user)
        {
            // Find the latest CreatedOn date
            var latestCreatedOn = _context.Package.Max(e => e.CreatedOn);

            try
            {
                var data = _context.Package.Where(e => (e.CreatedOn == latestCreatedOn && e.NT_User == user)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function GetDataByLatestandUser(string user): {ex.Message}");
            }
        }


        //Function to create the main package and sending the email
        [HttpPost]
        public async Task<ActionResult<List<Package>>> CreatePackage(Package package)
        {
            //Adding the package to the database
            package.StartDate = package.StartDate.AddHours(3);
            package.EndDate = package.EndDate.AddHours(3);
            _context.Package.Add(package);
            await _context.SaveChangesAsync();

           
            return Ok(await _context.Package.ToListAsync());
        }

        //Function to move the accepted employee package to history for better history channel
        [HttpPost("CreatePackageHistory")]
        public async Task<ActionResult<List<PackageHistory>>> CreatePackageHistory(PackageHistory package)
        {
            try
            {
                _context.PackageHistory.Add(package);
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function CreatePackageHistory(PackageHistory package): {ex.Message}");
            }


        }

        //Function to update the number of totalemployees when the package is partially accepted
        [HttpPut("EditTotalEmployeesInPackage/{acceptedEmployees}")]
        public async Task<ActionResult<List<Package>>> EditTotalEmployeesInPackage(Package package, int acceptedEmployees)
        {
            try
            {

                var dbTable = await _context.Package.FindAsync(package.PackageUID);
                if (dbTable == null)
                    return BadRequest("Not Found");


                dbTable.TotalEmployees = acceptedEmployees;

                await _context.SaveChangesAsync();
                return Ok(await _context.Package.ToListAsync());

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function EditTotalEmployeesInPackage(Package package, int acceptedEmployees): {ex.Message}");
            }

        }

        //Function to delete the package when totalemployees = 0
        [HttpDelete("{packageUID}")]
        public async Task<ActionResult<List<Package>>> DeletePackage(Guid packageUID)
        {
            try
            {

                var dbTable = await _context.Package.FindAsync(packageUID);
                if (dbTable == null)
                    return BadRequest("Not Found");

                _context.Package.Remove(dbTable);
                await _context.SaveChangesAsync();

                return Ok(await _context.Package.ToListAsync());

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error from function DeletePackage(Guid PackageUID): {ex.Message}");
            }


        }








        //Function to send the email when a new entry is made 
        private void SendMailMessage(string From, string SendTo, string ccc, string Subject, string Body, bool IsBodyHtml, string Server)
        {
            if (Body.Length > 0)
            {
                var pwd = _context.EMailTable.FirstOrDefault().Password;
                var mailkey = _config["Email:mailkey"];
                var password = DecryptString(mailkey, pwd);
                MailMessage htmlMessage;
                SmtpClient mySmtpClient;
                htmlMessage = new MailMessage();
                foreach (var address in SendTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    htmlMessage.To.Add(address);
                }
                if (ccc.Length > 0)
                {
                    htmlMessage.CC.Add(ccc);
                }
                htmlMessage.From = new MailAddress(From);
                htmlMessage.IsBodyHtml = IsBodyHtml;
                htmlMessage.Subject = Subject;
                htmlMessage.Body = Body;
                mySmtpClient = new SmtpClient(Server);
                //var pswd = Dts.Variables["$Project::MailPassword"].Value.ToString();
                mySmtpClient.UseDefaultCredentials = false;
                mySmtpClient.Port = 25;
                mySmtpClient.EnableSsl = true;
                mySmtpClient.Credentials = new NetworkCredential(From, password);
                mySmtpClient.Send(htmlMessage);
            }
        }

        //Starting to create the email 
        //Getting the contact info from the database
        /*  var fromUser = _context.EMailTable.FirstOrDefault().FromUser;
          var toUser = _context.EMailTable.FirstOrDefault().ToUser;
          var ccUser = _context.EMailTable.FirstOrDefault().CCUser;
          var mailkey = _config["Email:mailkey"];
          var decryptedFromUser = DecryptString(mailkey, fromUser);


          //Setting up customized subject based table type
          var tableType = package.Type;
          string subject = "";
          if (tableType == 1)
          {
              subject = "RTWA | New Personnel Demand";
          }
          else if (tableType == 2)
          {
              subject = "RTWA | New Personnel Offer";
          }

          //Setting the required variables
          var reporterName = package.ReporterName;
          var createdBy = package.NT_User;
          var department = package.Department;
          var shift = package.Shift;
          var functions = package.Functions;
          var competences = package.Competences;
          var startDate = package.StartDate;
          var endDate = package.EndDate;
          var totalDays = package.TotalDays;
          var totalEmployees = package.TotalEmployees;

          string body = "<style>\n  table {\n    border-collapse: collapse;\n    width: 100%;\n  }\n  th, td {\n    border: 1px solid #dddddd;\n    text-align: left;\n    padding: 8px;\n  }\n  th {\n    background-color: #f2f2f2;\n  }\n</style>" +
               "<h2>" + subject + "</h2>" +
               "<table><thead>" +
               "<tr>" +
               "<th>Created By</th> " +
               "<th>Department</th> " +
               "<th>Shift</th> " +
               "<th>Functions</th> " +
               "<th>Competences</th> " +
               "<th>Start Date</th> " +
               "<th>End Date</th> " +
               "<th>Total Days</th> " +
               "<th>Total Employees</th> " +
               "</tr>" +
               "</thead>" +

               "<tbody><tr>" +
               "<td>" +
               createdBy + "</td> <td>" +
               department + "</td> <td>" +
               shift + "</td> <td>" +
               functions + "</td> <td>" +
               competences + "</td> <td>" +
               startDate + "</td> <td>" +
               endDate + "</td> <td>" +
               totalDays + "</td> <td>" +
               totalEmployees +
               "</td> " +
               "</tr> </tbody> </table>" +
               "<br>" +
               "<a href= 'https://bljvm105.emea.bosch.com/RTWA_STAGING/'>Go to RTWA</a>";




          SendMailMessage(decryptedFromUser, toUser, ccUser, subject, body, true, "rb-smtp-auth.rbesz01.com");*/

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            string abc = Convert.ToBase64String(array);
            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }




    }
}
