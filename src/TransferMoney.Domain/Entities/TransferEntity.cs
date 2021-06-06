using System;
using System.ComponentModel.DataAnnotations;

namespace TransferMoney.Domain.Entities
{
    public class TransferEntity
    {
        [Key]
        public Guid TransactionId { get; set; }
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public double Value { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
