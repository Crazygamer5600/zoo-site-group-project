using System;
using System.Collections.Generic;

namespace ProjectStudioApp.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public DateTime EventStart { get; set; }

    public DateTime EventEnd { get; set; }

    public int MaxAttendeeSize { get; set; }

    public string EventPhoto { get; set; } = null!;

    public string EventType { get; set; } = null!;

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
