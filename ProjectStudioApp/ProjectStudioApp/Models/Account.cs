using System;
using System.Collections.Generic;

namespace ProjectStudioApp.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;
}
