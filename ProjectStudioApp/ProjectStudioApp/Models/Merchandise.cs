using System;
using System.Collections.Generic;

namespace ProjectStudioApp.Models;

public partial class Merchandise
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string ItemDescription { get; set; } = null!;

    public decimal ItemCost { get; set; }

    public string ItemImage { get; set; } = null!;
}
