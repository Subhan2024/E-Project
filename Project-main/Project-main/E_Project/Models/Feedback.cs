using System;
using System.Collections.Generic;

namespace E_Project.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public string FeedbackContent { get; set; } = null!;

    public DateTime FeedbackDate { get; set; }

    public virtual User? User { get; set; }
}
