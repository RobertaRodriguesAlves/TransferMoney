using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Service.Services
{
    public class AccountInformationService : IAccountInformationService
    {
        public async Task<ResponseDto> GetAccountInformation(TransferEntity transferDto)
        {
            var responseDto = new ResponseDto();

            var accountOrigin = await ChecksIfAccountExists(transferDto.AccountOrigin);
            var accountDestination = await ChecksIfAccountExists(transferDto.AccountDestination);
            if (accountOrigin != null && accountDestination != null)
            {
                if (CheckTheBalance(accountOrigin.balance, transferDto.Value))
                {
                    responseDto.Status = "Confirmed";
                }
                else
                {
                    responseDto.Status = "Error";
                    responseDto.Message = "Balance problem";
                }
            }
            else
            {
                responseDto.Status = "Error";
                responseDto.Message = "Inavalid account number";
            }
            return responseDto;
        }

        private static async Task<AccountDto> ChecksIfAccountExists(string account)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync($"https://acessoaccount.herokuapp.com/api/Account/{account}");
            string json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountDto>(json);
            return result;
        }

        private static bool CheckTheBalance(double balance, double balanceOfTheTransfer)
        {
            return balance >= balanceOfTheTransfer;
        }
    }
}
