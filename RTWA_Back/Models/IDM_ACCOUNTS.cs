using Microsoft.AspNetCore.Authorization;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace RTWA_Back.Models;
public class IDM_ACCOUNTS
{
  [Key, AutoIncrement] public int? Account_Id { get; set; } = null;
   
    public string? FullName { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public string? Password { get; set; } = null!;

    public DateTime? Lastmodified { get; set; } = null!;




}

