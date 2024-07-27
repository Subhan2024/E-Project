using System;
using System.Collections.Generic;

namespace E_Project.Models;

public partial class SendEmailList
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public int? UserId { get; set; }

    public int? TempImageId { get; set; }

    public virtual UserTemplateImage? TempImage { get; set; }

    public virtual User? User { get; set; }
}
