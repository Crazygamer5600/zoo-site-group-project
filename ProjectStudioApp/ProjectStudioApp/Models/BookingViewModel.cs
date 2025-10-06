using System.ComponentModel.DataAnnotations;

namespace ProjectStudioApp.Models.ViewModels;

public class BookingViewModel
{
    
    public int AdultTickets { get; set; } = 0;
    public int ChildTickets { get; set; } = 0;
    public int FamilyPackages { get; set; } = 0;

   
    public int BookingId { get; set; }
    public decimal TotalAmount { get; set; }

    
    [Display(Name = "MockCardNumber")]
    public string MockCardNumber { get; set; }
}