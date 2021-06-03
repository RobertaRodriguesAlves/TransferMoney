using System.Threading.Tasks;
using TransferMoney.Domain.DTO;

namespace TransferMoney.Domain.Interfaces
{
    public interface IAccountInformationService
    {
        bool GetAccountInformation(TransferDto transferDto);
    }
}