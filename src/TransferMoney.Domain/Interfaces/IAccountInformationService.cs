﻿using System.Threading.Tasks;
using TransferMoney.Domain.DTO;

namespace TransferMoney.Domain.Interfaces
{
    public interface IAccountInformationService
    {
        Task<bool> GetAccountInformation(TransferDto transferDto);
    }
}