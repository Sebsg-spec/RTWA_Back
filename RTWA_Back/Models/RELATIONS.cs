using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTWA_Back.Models;

public class RELATIONS
{
    [Key]
    public long? Role_Id { get; set; }

    
    
    public string? Account_Id { get; set; } = null;

    public DateTime? Lastmodified { get; set; } = null!;
}


