using System.ComponentModel.DataAnnotations;

namespace TransferMoney.Domain.DTO
{
    public class TransferDto
    {
        [Required(ErrorMessage = "Account origin is required")]
        public string accountOrigin { get; set; }

        [Required(ErrorMessage = "Account destination is required")]
        public string accountDestination { get; set; }

        [Required(ErrorMessage = "Value of transfer is required")]
        public double value { get; set; }
    }
}
