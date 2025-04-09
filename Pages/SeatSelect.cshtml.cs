using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MobileCheckIn.Pages
{
    public class SeatsModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public SeatsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string SelectedSeat { get; set; }

        public string ReservationNumber { get; set; }
        public string FlightDateString { get; set; }

        public DateTime ExpireTime { get; set; }
        public List<string> ReservedSeats { get; set; } = new List<string> { "12C", "16D", "25A" };

        public void OnGet()
        {
            ReservationNumber = TempData["ReservationNumber"]?.ToString() ?? "";
            FlightDateString = TempData["FlightDateString"]?.ToString() ?? "";

            ViewData["ExpireTime"] = DateTime.UtcNow.AddMinutes(5);
            SelectedSeat = string.Empty;
            TempData.Keep();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(SelectedSeat))
            {
                ModelState.AddModelError(string.Empty, "좌석을 선택해주세요.");
                TempData.Keep();
                return Page();
            }

            TempData["SelectedSeat"] = SelectedSeat;

            // DB에 좌석 반영
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                using (var cmd = new SqlCommand("UPDATE Reservations SET SeatNumber = @seat WHERE ReservationNo = @resNo", conn))
                {
                    cmd.Parameters.AddWithValue("@seat", SelectedSeat);
                    cmd.Parameters.AddWithValue("@resNo", TempData["ReservationNumber"]);

                    cmd.ExecuteNonQuery();
                }
            }

            TempData.Keep();

            var flightType = TempData["FlightType"] as string ?? "";

            if (flightType == "국제선")
                return RedirectToPage("Additional");
            else
                return RedirectToPage("Confirm");
        }

    }
}
