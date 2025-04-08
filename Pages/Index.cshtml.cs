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
            {
                Console.WriteLine("모델 상태가 유효하지 않습니다.");
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
