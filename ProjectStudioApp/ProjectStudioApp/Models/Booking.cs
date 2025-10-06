using System;
using System.Collections.Generic;

namespace ProjectStudioApp.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int ReservationSize { get; set; }

    public int AccountId { get; set; }

    
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = "PENDING";
    public string TransactionReference { get; set; }
    

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}