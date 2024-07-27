using System;
using System.Collections.Generic;

namespace E_Project.Models;

public partial class SubscriptionDetail
{
    public int SubscriptionId { get; set; }

    public DateOnly SubscriptionStartDate { get; set; }

    public DateOnly SubscriptionEndDate { get; set; }

    public string Email { get; set; } = null!;

    public string Status { get; set; } = null!;
}
