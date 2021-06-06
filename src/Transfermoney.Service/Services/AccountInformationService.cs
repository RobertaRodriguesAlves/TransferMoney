using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Service.Services
{
    public class AccountInformationService : IAccountInformationService
    {
        public async Task<FullResponseDto> GetAccountInformation(TransferEntity transfer)
        {
            var fullresponse = new FullResponseDto();

            try
            {
                var accountOrigin = await ChecksIfAccountExists(transfer.AccountOrigin);
                if (accountOrigin.accountNumber == null)
                {
                    fullresponse.Status = "Error";
                    fullresponse.Message = "AccountOrigin doesn't exist";
                    return fullresponse;
                }

                var accountDestination = await ChecksIfAccountExists(transfer.AccountDestination);
                if (accountDestination.accountNumber == null)
                {
                    fullresponse.Status = "Error";
                    fullresponse.Message = "AccountDestination doesn't exist";
                    return fullresponse;
                }

                if (CheckTheBalance(accountOrigin.balance, transfer.Value))
                {
                    MakesCreditAndDebitOperation(transfer);
                    fullresponse.Status = "Confirmed";
                }
                else
                {
                    fullresponse.Status = "Error";
                    fullresponse.Message = "AccountOrigin doesn't have enough money";
                    return fullresponse;
                }
            }
            catch (Exception ex)
            {
                fullresponse.Status = "Error";
                fullresponse.Message = ex.Message;
            }

            return fullresponse;
        }

        private static void MakesCreditAndDebitOperation(TransferEntity transfer)
        {
            for (int i = 0; i < 2; i++)
            {
                OperationDto operation = FillObjectToSerialize(transfer, i);
                string requestUri = "https://acessoaccount.herokuapp.com/api/Account";
                HttpResponseMessage response;
                string jsonBody = JsonConvert.SerializeObject(operation);
                HttpContent body = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    response = client.PostAsync(requestUri, body).Result;

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(response.Content.ReadAsStringAsync().Result);
                    }
                }
            } 
        }

        private static OperationDto FillObjectToSerialize(TransferEntity transfer, int i)
        {
            if(i == 0)
            {
                return new OperationDto
                {
                    AccountNumber = transfer.AccountOrigin,
                    Value = transfer.Value,
                    Type = "Debit"
                };
            }

            return new OperationDto
            {
                AccountNumber = transfer.AccountDestination,
                Value = transfer.Value,
                Type = "Credit"
            };
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
