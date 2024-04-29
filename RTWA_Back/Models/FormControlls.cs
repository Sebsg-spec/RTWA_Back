using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTWA_Back.Models
{
    public class FormControlls
    {
        public string? Type { get; set; }

        public string? Value { get; set; }

        [Key] public long? Id {  get; set; }


    }
}
