using Microsoft.EntityFrameworkCore;
using TransferMoney.Domain.Entities;

namespace TransferMoney.Data.Context
{
    public class TransferMoneyDbContext : DbContext
    {
        public DbSet<TransferEntity> Transfers { get; set; }

        public TransferMoneyDbContext(DbContextOptions<TransferMoneyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TransferEntity>();
        }
    }
}
