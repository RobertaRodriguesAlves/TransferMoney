using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransferMoney.Domain.Entities;

namespace TransferMoney.Data.Mapping
{
    public class TransferMoneyMap : IEntityTypeConfiguration<TransferEntity>
    {
        public void Configure(EntityTypeBuilder<TransferEntity> builder)
        {
            builder.ToTable("Transfers");
            builder.HasKey(transfer => transfer.TransactionId);
            builder.HasIndex(transfer => transfer.TransactionId).IsUnique();
            builder.Property(transfer => transfer.AccountOrigin).IsRequired().HasMaxLength(8);
            builder.Property(transfer => transfer.AccountDestination).IsRequired().HasMaxLength(8);
            builder.Property(transfer => transfer.Value).IsRequired();
        }
    }
}
