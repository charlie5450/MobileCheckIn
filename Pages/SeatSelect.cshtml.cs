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

        // ���õ� �¼����� ��Ȱ��ȭ�� ���
        public List<string> ReservedSeats { get; set; } = new List<string> { "12C", "16D", "25A" };

        public void OnGet()
        {
            ReservationNumber = TempData.ContainsKey("ReservationNumber") ? TempData["ReservationNumber"]?.ToString() ?? "" : "";
            FlightDateString = TempData.ContainsKey("FlightDateString") ? TempData["FlightDateString"]?.ToString() ?? "" : "";

            SelectedSeat = string.Empty;

            TempData.Keep(); // �����ȣ, ��¥ ����
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(SelectedSeat))
            {
                ModelState.AddModelError(string.Empty, "�¼��� �������ּ���.");
                return Page();
            }

            TempData["SelectedSeat"] = SelectedSeat;
            TempData.Keep();

            var flightType = TempData["FlightType"] as string ?? "";

            if (flightType == "������")
                return RedirectToPage("Additional"); // �������̸� ���� ��������
            else
                return RedirectToPage("Confirm"); // �������̸� �ٷ� Confirm
        }
    }
}
