using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MobileCheckIn.Pages
{
    public class SeatsModel : PageModel
    {
        [BindProperty]
        public string SelectedSeat { get; set; }

        // �� ������Ƽ���� TempData���� ���� �Ҵ�
        public string ReservationNumber { get; set; }
        public string FlightDateString { get; set; }

        public void OnGet()
        {
            // TempData�� ���� ���ٸ� "" ��ȯ (as string �� null�̸� ���� ����)
            var reservation = TempData.ContainsKey("ReservationNumber") ? TempData["ReservationNumber"]?.ToString() : "";
            var flightDate = TempData.ContainsKey("FlightDateString") ? TempData["FlightDateString"]?.ToString() : "";

            ReservationNumber = reservation;
            FlightDateString = flightDate;

            // TempData ���� ���� ��û������ ���� (�ʼ�)
            TempData.Keep();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(SelectedSeat))
            {
                ModelState.AddModelError(string.Empty, "�¼��� �������ּ���.");
                return Page();
            }

            TempData["SelectedSeat"] = SelectedSeat;

            // ���⵵ TempData ��� ���� (Confirm ���������� �� ��� �ϴϱ�)
            TempData.Keep();

            var flightType = TempData["FlightType"] as string ?? "";
            if (flightType == "������")
                return RedirectToPage("Additional"); //�������̸� ���� ��������
            else
                return RedirectToPage("Confirm"); //�������̸� �ٷ� Confirm
        }
    }

}
