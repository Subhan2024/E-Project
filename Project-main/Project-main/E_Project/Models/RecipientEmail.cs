using System;
using System.Collections.Generic;

namespace E_Project.Models;

public partial class RecipientEmail
{
    public int RecipientId { get; set; }

    public int? UserId { get; set; }

    public string Email { get; set; } = null!;

    public virtual User? User { get; set; }
}
