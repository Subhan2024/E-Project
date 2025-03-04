﻿using System;
using System.Collections.Generic;

namespace E_Project.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public DateTime? CreateDate { get; set; }
}
