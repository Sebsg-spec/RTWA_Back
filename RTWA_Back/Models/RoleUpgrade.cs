namespace RTWA_Back.Models
{
    public class RoleUpgrade
    {
        public long? Id { get; set; }
        public string? RoleRequested { get; set; }
        public string? Reason { get; set; }
        public long? RoleRequestedId { get; set; }
        public string? Account_id { get; set; }

    }
}
