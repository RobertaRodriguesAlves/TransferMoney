using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Interfaces;

namespace TransferMoney.Service.Services
{
    public class FundTransferService : IFundTransferService
    {
        private readonly IAccountInformationService _accountInformationService;

        public FundTransferService(IAccountInformationService accountInformationService)
        {
            _accountInformationService = accountInformationService;
        }

        public Task<string> Get(string transactionId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Post(TransferDto transfer)
        {
            //recebo a transacao
            //envio para fila



            //retorno id


            var result = await _accountInformationService.GetAccountInformation(transfer);



            return true;
        }
    }
}
