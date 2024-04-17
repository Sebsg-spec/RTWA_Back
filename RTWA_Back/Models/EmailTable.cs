using SQLite;
using System.ComponentModel.DataAnnotations;

namespace RTWA_Back.Models
{
    public class EMailTable
    {
        [Key, AutoIncrement] public int ID { get; set; }
        public string? FromUser { get; set; }
        public string? ToUser { get; set; }
        public string? CCUser { get; set; }
        public string?  Password { get; set; }
    } 
}
