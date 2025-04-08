using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MobileCheckIn.Pages
{
    public class SeatsModel : PageModel
    {
        [BindProperty]
        public string SelectedSeat { get; set; }

        // 이 프로퍼티들은 TempData에서 직접 할당
        public string ReservationNumber { get; set; }
        public string FlightDateString { get; set; }

        public void OnGet()
        {
            // TempData에 값이 없다면 "" 반환 (as string 은 null이면 문제 생김)
            var reservation = TempData.ContainsKey("ReservationNumber") ? TempData["ReservationNumber"]?.ToString() : "";
            var flightDate = TempData.ContainsKey("FlightDateString") ? TempData["FlightDateString"]?.ToString() : "";

            ReservationNumber = reservation;
            FlightDateString = flightDate;

            // TempData 값을 다음 요청에서도 유지 (필수)
            TempData.Keep();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(SelectedSeat))
            {
                ModelState.AddModelError(string.Empty, "좌석을 선택해주세요.");
                return Page();
            }

            TempData["SelectedSeat"] = SelectedSeat;

            // 여기도 TempData 계속 유지 (Confirm 페이지에서 또 써야 하니까)
            TempData.Keep();

            var flightType = TempData["FlightType"] as string ?? "";
            if (flightType == "국제선")
                return RedirectToPage("Additional"); //국제선이면 여권 페이지로
            else
                return RedirectToPage("Confirm"); //국내선이면 바로 Confirm
        }
    }

}
