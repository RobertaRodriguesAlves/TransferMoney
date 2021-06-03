using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Service.Services
{
    public class AccountInformationService : IAccountInformationService
    {
        public bool GetAccountInformation(TransferDto transferDto)
        { 
            return true;
        }

        private static async Task<AccountDtoResult> ChecksIfAccountExists(string accountOrigin, string accountDestination)
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync($"https://acessoaccount.herokuapp.com/api/Account/{accountOrigin}");
            string json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountDtoResult>(json);
            return result;
        }

        private static bool CheckTheBalance(double balance, double balanceOfTheTransfer)
        {
            return balanceOfTheTransfer >= balance;
        }
    }
}
