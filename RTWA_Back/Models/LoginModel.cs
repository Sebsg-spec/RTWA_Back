using System.ComponentModel.DataAnnotations;

namespace RTWA_Back.Models
{
    public class LoginModel
    {
     [Key]   public int? Account_Id { get; set; } = null;
        public string FullName { get; set; }
        public string Email { get; set; }

        public bool Result { get; set; }
        public string Message { get; set; }
        public string password { get; set; }

    }
}
