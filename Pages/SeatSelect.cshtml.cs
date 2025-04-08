using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MobileCheckIn.Pages
{
    public class SeatsModel : PageModel
    {
        [BindProperty]
        public string SelectedSeat { get; set; }

        public string ReservationNumber { get; set; }
        public string FlightDateString { get; set; }

        // 선택된 좌석들을 비활성화할 목록
        public List<string> ReservedSeats { get; set; } = new List<string> { "12C", "16D", "25A" };

        public void OnGet()
        {
            ReservationNumber = TempData.ContainsKey("ReservationNumber") ? TempData["ReservationNumber"]?.ToString() ?? "" : "";
            FlightDateString = TempData.ContainsKey("FlightDateString") ? TempData["FlightDateString"]?.ToString() ?? "" : "";

            SelectedSeat = string.Empty;

            TempData.Keep(); // 예약번호, 날짜 유지
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(SelectedSeat))
            {
                ModelState.AddModelError(string.Empty, "좌석을 선택해주세요.");
                return Page();
            }

            TempData["SelectedSeat"] = SelectedSeat;
            TempData.Keep();

            var flightType = TempData["FlightType"] as string ?? "";

            if (flightType == "국제선")
                return RedirectToPage("Additional"); // 국제선이면 여권 페이지로
            else
                return RedirectToPage("Confirm"); // 국내선이면 바로 Confirm
        }
    }
}
