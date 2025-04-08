using System.ComponentModel.DataAnnotations;

namespace MobileCheckIn.Models
{
    public class Visitor
    {
        [Required(ErrorMessage = "예약번호를 입력해주세요.")]
        public string ReservationNumber { get; set; }

        [Required(ErrorMessage = "탑승일을 선택해주세요.")]
        [DataType(DataType.Date)]
        public DateTime? FlightDate { get; set; }

        [Required(ErrorMessage = "노선을 선택해주세요.")]
        public string FlightType { get; set; }
    }
}

