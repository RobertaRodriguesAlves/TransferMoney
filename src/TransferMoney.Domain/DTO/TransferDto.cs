using System.ComponentModel.DataAnnotations;

namespace TransferMoney.Domain.DTO
{
    public class TransferDto
    {
        [Required(ErrorMessage = "Account origin is required")]
        public string AccountOrigin { get; set; }

        [Required(ErrorMessage = "Account destination is required")]
        public string AccountDestination { get; set; }

        [Required(ErrorMessage = "Value of transfer is required")]
        public double Value { get; set; }
    }
}
