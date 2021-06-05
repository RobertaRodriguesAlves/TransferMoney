using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Service.Services
{
    public class AccountInformationService : IAccountInformationService
    {
        public async Task<bool> GetAccountInformation(TransferDto transferDto)
        {
            var accountOrigin = await ChecksIfAccountExists(transferDto.accountOrigin);
            var accountDestination = await ChecksIfAccountExists(transferDto.accountDestination);
            if (accountOrigin != null && accountDestination != null)
                return CheckTheBalance(accountOrigin.balance, transferDto.value);

            return false;
        }

        private static async Task<AccountDtoResult> ChecksIfAccountExists(string account)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync($"https://acessoaccount.herokuapp.com/api/Account/{account}");
            string json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountDtoResult>(json);
            return result;
        }

        private static bool CheckTheBalance(double balance, double balanceOfTheTransfer)
        {
            return balance >= balanceOfTheTransfer;
        }
    }
}
