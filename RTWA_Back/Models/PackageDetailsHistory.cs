using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RTWA_Back.Models
{
    public class PackageDetailsHistory
    {

        [Key, AutoIncrement] public int Id { get; set; }
        public string Functions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Column(TypeName = "decimal(18,4)")] public decimal TotalDays { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Competences { get; set; }
        public string? Shift { get; set; }
        public Guid PackageId { get; set; }
        public string? NT_User { get; set; }
        [Column(TypeName = "decimal(18,4)")] public decimal? Status { get; set; }
        public string? ReporterName { get; set; }
        /*        public RequestTables RequestTable { get; set; }*/

    }
}
