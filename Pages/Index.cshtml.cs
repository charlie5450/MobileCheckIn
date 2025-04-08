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
            {
                Console.WriteLine("�� ���°� ��ȿ���� �ʽ��ϴ�.");
                return Page();
            }
            
            var flightDateStr = Visitor.FlightDate?.ToString("yyyy-MM-dd") ?? "";            

            TempData["ReservationNumber"] = Visitor.ReservationNumber ?? "";
            TempData["FlightDateString"] = flightDateStr;
            TempData["FlightType"] = Visitor.FlightType ?? "";

            return RedirectToPage("SeatSelect");
        }

    }
}
