using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTWA_Back.Models
{
    public class FormControlls
    {
        public string? Type { get; set; }

        public string? Value { get; set; }

        [Key, AutoIncrement] public int? Id {  get; set; }


    }
}
