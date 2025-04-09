using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MobileCheckIn.Models;
using System.Data.SqlClient;

namespace MobileCheckIn.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [BindProperty]
        public Visitor Visitor { get; set; } = new Visitor();

        public void OnGet()
        {
            // �ʱ� ������ �ε�
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string reservationNo = Visitor.ReservationNumber ?? "";
            DateTime? flightDate = Visitor.FlightDate;

            if (string.IsNullOrEmpty(reservationNo) || flightDate == null)
            {
                ModelState.AddModelError(string.Empty, "�����ȣ�� ž������ �Է����ּ���.");
                return Page();
            }

            // DB ��ȸ
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            bool isInternational = false;
            bool found = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT IsInternational 
                                 FROM Reservations 
                                 WHERE ReservationNo = @ReservationNo AND FlightDate = @FlightDate";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationNo", reservationNo);
                    cmd.Parameters.AddWithValue("@FlightDate", flightDate.Value);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        isInternational = (bool)result;
                        found = true;
                    }
                }
            }

            if (!found)
            {
                ModelState.AddModelError(string.Empty, "��ġ�ϴ� ���� ������ �����ϴ�.");
                return Page();
            }

            // TempData�� ����
            TempData["ReservationNumber"] = reservationNo;
            TempData["FlightDateString"] = flightDate?.ToString("yyyy-MM-dd");
            TempData["FlightType"] = isInternational ? "������" : "������";

            return RedirectToPage("SeatSelect");
        }
    }
}
