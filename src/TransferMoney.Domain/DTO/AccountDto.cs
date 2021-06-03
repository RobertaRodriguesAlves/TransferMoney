using System.ComponentModel.DataAnnotations;

namespace TransferMoney.Domain.DTO
{
    public class AccountDto
    {
        //[Required(ErrorMessage = "Account number is required")]
        public string accountNumber { get; set; }

        //[Required(ErrorMessage = "Amount of a balance for the account is required.")]
        //public double value { get; set; }

        //[Required(ErrorMessage = "Account type is required")]
        //public string type { get; set; }
    }
}
