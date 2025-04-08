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
            // 초기 페이지 로드 시 실행
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // 로그로 확인
            var flightDateStr = Visitor.FlightDate?.ToString("yyyy-MM-dd") ?? "";
            Console.WriteLine("탑승일 저장 (TempData): " + flightDateStr);

            TempData["ReservationNumber"] = Visitor.ReservationNumber ?? "";
            TempData["FlightDateString"] = flightDateStr;

            return RedirectToPage("SeatSelect");
        }

    }
}
