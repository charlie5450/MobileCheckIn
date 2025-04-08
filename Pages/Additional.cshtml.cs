using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MobileCheckIn.Pages
{
    public class AdditionalModel : PageModel
    {
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

            TempData.Keep();
            return RedirectToPage("Confirm");
        }
    }
}
