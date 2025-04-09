using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MobileCheckIn.Pages
{
    public class AdditionalModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public AdditionalModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string Gender { get; set; }

        [BindProperty]
        public DateTime? DateOfBirth { get; set; }

        [BindProperty]
        public string Nationality { get; set; }

        [BindProperty]
        public string PassportNumber { get; set; }

        [BindProperty]
        public DateTime? PassportExpiry { get; set; }

        public void OnGet()
        {
            TempData.Keep(); // keep previous data

            var reservationNo = TempData["ReservationNumber"]?.ToString();
            if (string.IsNullOrEmpty(reservationNo))
                return;

            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            SELECT LastName, FirstName, Gender, DateOfBirth, Nationality, PassportNumber, PassportExpiry
            FROM Reservations
            WHERE ReservationNo = @ReservationNo", conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationNo", reservationNo);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            LastName = reader["LastName"]?.ToString();
                            FirstName = reader["FirstName"]?.ToString();
                            Gender = reader["Gender"]?.ToString();
                            DateOfBirth = reader["DateOfBirth"] as DateTime?;
                            Nationality = reader["Nationality"]?.ToString();
                            PassportNumber = reader["PassportNumber"]?.ToString();
                            PassportExpiry = reader["PassportExpiry"] as DateTime?;
                        }
                    }
                }
            }
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid ||
                string.IsNullOrEmpty(LastName) ||
                string.IsNullOrEmpty(FirstName) ||
                string.IsNullOrEmpty(Gender) ||
                DateOfBirth == null ||
                string.IsNullOrEmpty(Nationality) ||
                string.IsNullOrEmpty(PassportNumber) ||
                PassportExpiry == null)
            {
                ModelState.AddModelError(string.Empty, "모든 정보를 정확히 입력해주세요.");
                TempData.Keep();
                return Page();
            }

            TempData["LastName"] = LastName;
            TempData["FirstName"] = FirstName;
            TempData["Gender"] = Gender;
            TempData["DateOfBirth"] = DateOfBirth?.ToString("yyyy-MM-dd");
            TempData["Nationality"] = Nationality;
            TempData["PassportNumber"] = PassportNumber;
            TempData["PassportExpiry"] = PassportExpiry?.ToString("yyyy-MM-dd");

            // DB 업데이트
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
            UPDATE Reservations SET 
                LastName = @LastName,
                FirstName = @FirstName,
                Gender = @Gender,
                DateOfBirth = @DateOfBirth,
                Nationality = @Nationality,
                PassportNumber = @PassportNumber,
                PassportExpiry = @PassportExpiry
            WHERE ReservationNo = @ReservationNo", conn))
                {
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    cmd.Parameters.AddWithValue("@Nationality", Nationality);
                    cmd.Parameters.AddWithValue("@PassportNumber", PassportNumber);
                    cmd.Parameters.AddWithValue("@PassportExpiry", PassportExpiry);
                    cmd.Parameters.AddWithValue("@ReservationNo", TempData["ReservationNumber"]);

                    cmd.ExecuteNonQuery();
                }
            }

            TempData.Keep();
            return RedirectToPage("Confirm");
        }

    }
}
