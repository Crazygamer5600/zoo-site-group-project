using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectStudioApp.Datafile;
using ProjectStudioApp.Models;
using ProjectStudioApp.Models.ViewModels;

namespace ProjectStudioApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ZooliranteDbContext _context;

        private readonly decimal AdultPrice = 45.00m;
        private readonly decimal ChildPrice = 25.00m;
        private readonly decimal FamilyPackagePrice = 120.00m;

        public BookingsController(ZooliranteDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(new BookingViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel viewModel)
        {
            // 1. Login Check: If not logged in, redirect to login page
            if (LoggedInUser.CurrentAccount == null)
            {
                
                TempData["ErrorMessage"] = "You must be logged in to complete a booking.";
                return RedirectToAction("Index", "Accounts");
            }
            int currentAccountId = LoggedInUser.CurrentAccount.AccountId;

            // 2. Business Logic Validation: Check ticket count
            if (viewModel.AdultTickets <= 0 && viewModel.ChildTickets <= 0 && viewModel.FamilyPackages <= 0)
            {
               
                ModelState.AddModelError("", "Please select at least one ticket.");
                return View("Index", viewModel);
            }

            // 3. Calculate total and reservation size
            decimal total = (viewModel.AdultTickets * AdultPrice) +
                            (viewModel.ChildTickets * ChildPrice) +
                            (viewModel.FamilyPackages * FamilyPackagePrice);

            int totalReservation = viewModel.AdultTickets + viewModel.ChildTickets + (viewModel.FamilyPackages * 4);

            // 4. Create Booking entity and assign values
            var booking = new Booking
            {
                AccountId = currentAccountId,
                ReservationSize = totalReservation,
                TotalAmount = total,
                PaymentStatus = "PENDING",
                TransactionReference = null
            };

            // 5. Save to database and catch exceptions
            try
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction("Checkout", new { id = booking.BookingId });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                
                string innerMessage = ex.InnerException?.Message ?? ex.Message;

                ModelState.AddModelError("", $"Order creation failed: Database constraint error. Details: {innerMessage}");

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError("", $"Order creation failed: Unknown error. Details: {ex.Message}");
                return View("Index", viewModel);
            }
        }

        

        
        
    }
}