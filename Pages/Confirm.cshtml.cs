using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MobileCheckIn.Pages
{
    public class ConfirmModel : PageModel
    {
        public string ReservationNumber { get; set; }
        public string FlightDateString { get; set; }
        public string SelectedSeat { get; set; }

        public void OnGet()
        {
            ReservationNumber = TempData["ReservationNumber"]?.ToString() ?? "";
            FlightDateString = TempData["FlightDateString"]?.ToString() ?? "";
            SelectedSeat = TempData["SelectedSeat"]?.ToString() ?? "";

            TempData.Keep(); // 마지막까지 TempData 유지
        }
    }


}
