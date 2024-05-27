using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = ServiceStack.DataAnnotations.RequiredAttribute;
using StringLengthAttribute = System.ComponentModel.DataAnnotations.StringLengthAttribute;

namespace RTWA_Back.Models;

public class ROLES
{

    [Key] public int Role_Id { get; set; }

    [Required]
    [StringLength(255)]
    public string role_name { get; set; } = null!;

    public DateTime? Lastmodified { get; set; } = null!;
}

