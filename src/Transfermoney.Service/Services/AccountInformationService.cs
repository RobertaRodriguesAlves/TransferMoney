using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;
using TransferMoney.Domain.Enums;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Service.Services
{
    public class AccountInformationService : IAccountInformationService
    {
        private readonly ILogger<AccountInformationService> _logger;
        public AccountInformationService(ILogger<AccountInformationService> logger)
        {
            _logger = logger;
        }
        public async Task<FullResponseDto> MakesAccountOperation(TransferEntity transfer)
        {
            _logger.LogInformation("Starting the accounts validation");
            var fullresponse = new FullResponseDto();

            try
            {
                _logger.LogInformation("Checking if accountOrigin exists");
                AccountDto accountOrigin = await ChecksIfAccountExists(transfer.AccountOrigin);
                if (accountOrigin.AccountNumber == null)
                {
                    fullresponse.Status = "Error";
                    fullresponse.Message = "AccountOrigin doesn't exist";
                    _logger.LogWarning($"{fullresponse.Status} - {fullresponse.Message}");
                    return fullresponse;
                }

                _logger.LogInformation("Checking if accountDestination exists");
                AccountDto accountDestination = await ChecksIfAccountExists(transfer.AccountDestination);
                if (accountDestination.AccountNumber == null)
                {
                    fullresponse.Status = "Error";
                    fullresponse.Message = "AccountDestination doesn't exist";
                    _logger.LogWarning($"{fullresponse.Status} - {fullresponse.Message}");
                    return fullresponse;
                }

                _logger.LogInformation("Checking the balance of accountOrigin");
                if (ChecksTheBalance(accountOrigin.Balance, transfer.Value))
                {
                    _logger.LogInformation("Starting the transfer operation");
                    MakesCreditAndDebitOperation(transfer);
                    fullresponse.Status = "Confirmed";
                    _logger.LogInformation($"Status of operation: {fullresponse.Status}");
                }
                else
                {
                    fullresponse.Status = "Error";
                    fullresponse.Message = "AccountOrigin doesn't have enough money";
                    _logger.LogWarning($"{fullresponse.Status} - {fullresponse.Message}");
                    return fullresponse;
                }
            }
            catch (Exception ex)
            {
                fullresponse.Status = "Error";
                fullresponse.Message = ex.Message;
                _logger.LogError($"{fullresponse.Status} - {fullresponse.Message}");
            }

            _logger.LogInformation("Finishing the accounts validation");
            return fullresponse;
        }

        private static void MakesCreditAndDebitOperation(TransferEntity transfer)
        {
            byte numberOfOperations = 2;
            for (int count = 0; count < numberOfOperations; count++)
            {
                OperationDto operation = FillObjectToSerialize(transfer, count);
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

        private static OperationDto FillObjectToSerialize(TransferEntity transfer, int count)
        {
            if(count == (int)Operation.Debit)
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
            AccountDto result = JsonConvert.DeserializeObject<AccountDto>(json);
            return result;
        }

        private static bool ChecksTheBalance(double balance, double balanceOfTheTransfer)
        {
            return balance >= balanceOfTheTransfer;
        }
    }
}
