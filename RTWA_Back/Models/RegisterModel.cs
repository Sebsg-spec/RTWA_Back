using System.ComponentModel.DataAnnotations;
using SQLite;

namespace RTWA_Back.Models
{
    public class RegisterModel
    {

        [Key, AutoIncrement]  public int AccountID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public string LastModified { get; set; }



    }
}

