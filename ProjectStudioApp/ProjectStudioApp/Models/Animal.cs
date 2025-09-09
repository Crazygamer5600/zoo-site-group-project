using System;
using System.Collections.Generic;

namespace ProjectStudioApp.Models;

public partial class Animal
{
    public int AnimalId { get; set; }

    public string Name { get; set; } = null!;

    public string Species { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ExtraInfo { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string AnimalPhoto { get; set; } = null!;

    public string AnimalLocation { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
