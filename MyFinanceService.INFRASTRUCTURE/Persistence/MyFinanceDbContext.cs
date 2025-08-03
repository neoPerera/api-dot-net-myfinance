using MyFinanceService.CORE.Entities;
using MyFinanceService.INFRASTRUCTURE.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MyFinanceService.INFRASTRUCTURE.Persistence
{
    public class MyFinanceDbContext : DbContext
    {
        public MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options) : base(options) { }

        // Define DB sets (tables)
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionRecord> TransactionRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
            modelBuilder.ApplyConfiguration(new IncomeConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionRecordConfiguration());
        }
    }
} 