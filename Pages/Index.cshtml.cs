using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileCheckIn.Models;

namespace MobileCheckIn.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public Visitor Visitor { get; set; } = new Visitor();

        public void OnGet()
        {
            // �ʱ� ������ �ε� �� ����
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // �α׷� Ȯ��
            var flightDateStr = Visitor.FlightDate?.ToString("yyyy-MM-dd") ?? "";
            Console.WriteLine("ž���� ���� (TempData): " + flightDateStr);

            TempData["ReservationNumber"] = Visitor.ReservationNumber ?? "";
            TempData["FlightDateString"] = flightDateStr;

            return RedirectToPage("SeatSelect");
        }

    }
}
