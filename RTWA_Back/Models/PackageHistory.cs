using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RTWA_Back.Models
{
    public class PackageHistory
    {
        [Key]
        public Guid PackageUID { get; set; }
        [Column(TypeName = "decimal(18,4)")] public decimal Type { get; set; }
        public string? ReporterName { get; set; }
        public string Department { get; set; }
        public string Functions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Column(TypeName = "decimal(18,4)")] public decimal TotalEmployees { get; set; }
        [Column(TypeName = "decimal(18,4)")] public decimal TotalDays { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Competences { get; set; }
        public string? Shift { get; set; }
        public string? NT_User { get; set; }
        [Column(TypeName = "decimal(18,4)")] public decimal? Status { get; set; }
    }
}
