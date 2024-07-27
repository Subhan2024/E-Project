using System;
using System.Collections.Generic;

namespace E_Project.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? SubscriptionStatus { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<RecipientEmail> RecipientEmails { get; set; } = new List<RecipientEmail>();

    public virtual ICollection<SendEmailList> SendEmailLists { get; set; } = new List<SendEmailList>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<UserTemplateImage> UserTemplateImages { get; set; } = new List<UserTemplateImage>();
}
