using System;
using System.Collections.Generic;

namespace E_Project.Models;

public partial class UserTemplateImage
{
    public int Id { get; set; }

    public string? UserTempImage { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<SendEmailList> SendEmailLists { get; set; } = new List<SendEmailList>();

    public virtual User? User { get; set; }
}
