using System.Threading.Tasks;
using TransferMoney.Domain.DTO;
using TransferMoney.Domain.Entities;

namespace TransferMoney.Domain.Interfaces
{
    public interface IAccountInformationService
    {
        Task<FullResponseDto> GetAccountInformation(TransferEntity transferDto);
    }
}