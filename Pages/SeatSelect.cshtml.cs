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
            TempData.Keep();

            // DB���� ���� �¼���ȣ ��ȸ
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT SeatNumber FROM Reservations WHERE ReservationNo = @resNo", conn))
                {
                    cmd.Parameters.AddWithValue("@resNo", ReservationNumber);

                    var seat = cmd.ExecuteScalar();
                    if (seat != null && seat != DBNull.Value)
                    {
                        SelectedSeat = seat.ToString();
                    }
                    else
                    {
                        SelectedSeat = string.Empty;
                    }
                }
            }

            // ����: �̹� ����� �¼� ����Ʈ (���� DB���� �ҷ������� Ȯ�� ����)
            ReservedSeats = new List<string> { "12C", "16D", "25A" };
        }


        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(SelectedSeat))
            {
                ModelState.AddModelError(string.Empty, "�¼��� �������ּ���.");
                TempData.Keep();
                return Page();
            }

            TempData["SelectedSeat"] = SelectedSeat;

            // DB�� �¼� �ݿ�
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

            if (flightType == "������")
                return RedirectToPage("Additional");
            else
                return RedirectToPage("Confirm");
        }

    }
}
