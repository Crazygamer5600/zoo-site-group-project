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
            if (LoggedInUser.CurrentAccount == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to complete a booking.";
                return RedirectToAction("Index", "Accounts");
            }
            int currentAccountId = LoggedInUser.CurrentAccount.AccountId;

            if (viewModel.AdultTickets <= 0 && viewModel.ChildTickets <= 0 && viewModel.FamilyPackages <= 0)
            {
                ModelState.AddModelError("", "Please select at least one ticket.");
                return View("Index", viewModel);
            }

           
            decimal total = (viewModel.AdultTickets * AdultPrice) +
                            (viewModel.ChildTickets * ChildPrice) +
                            (viewModel.FamilyPackages * FamilyPackagePrice);

            int totalReservation = viewModel.AdultTickets + viewModel.ChildTickets + (viewModel.FamilyPackages * 4);

            
            var booking = new Booking
            {
                AccountId = currentAccountId,
                ReservationSize = totalReservation
                
            };

            try
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();

                
                TempData["AdultTickets"] = viewModel.AdultTickets;
                TempData["ChildTickets"] = viewModel.ChildTickets;
                TempData["FamilyPackages"] = viewModel.FamilyPackages;


                return RedirectToAction("Checkout", new { id = booking.BookingId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Order creation failed: Unknown error. Details: {ex.Message}");
                return View("Index", viewModel);
            }
        }

       
        public async Task<IActionResult> Checkout(int id)
        {
            
            int adultTickets = TempData["AdultTickets"] as int? ?? 0;
            int childTickets = TempData["ChildTickets"] as int? ?? 0;
            int familyPackages = TempData["FamilyPackages"] as int? ?? 0;

          
            decimal recalculatedTotal = (adultTickets * AdultPrice) +
                                        (childTickets * ChildPrice) +
                                        (familyPackages * FamilyPackagePrice);

            
            var checkoutViewModel = new BookingViewModel
            {
                BookingId = id,
                TotalAmount = recalculatedTotal,
                AdultTickets = adultTickets,
                ChildTickets = childTickets,
                FamilyPackages = familyPackages
            };

           
            if (recalculatedTotal == 0)
            {
                TempData["ErrorMessage"] = "Order details are missing. Please re-select your tickets.";
                return RedirectToAction("Index");
            }

            return View(checkoutViewModel);
        }

      
        [HttpPost]
        public async Task<IActionResult> SimulatePayment([FromBody] PaymentRequest request)
        {
            
            if (request == null || request.BookingId <= 0)
            {
                return Json(new { success = false, message = "Invalid payment request." });
            }

           
            return Json(new
            {
                success = true,
                message = "Payment simulated successfully. Order confirmed.",
                transactionRef = Guid.NewGuid().ToString().Substring(0, 8)
            });
        }

        
        public async Task<IActionResult> Details(int? id) { return NotFound(); }
        public IActionResult Create() { return View(); }
        public async Task<IActionResult> Edit(int? id) { return NotFound(); }
    }

   
    public class PaymentRequest
    {
        public int BookingId { get; set; }
        public string MockCardNumber { get; set; }
    }
}