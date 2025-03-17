using CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace INFRASTRUCTURE.Persistence.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("tbl_refmas_expense");

            // Configure primary key
            builder.HasKey(e => e.Id);

            // Map properties to columns with specific names
            builder.Property(e => e.Id)
                .HasColumnName("str_id")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Name)
                .HasColumnName("str_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Active)
                .HasColumnName("str_active")
                .HasMaxLength(1)
                .IsRequired();

            builder.Property(e => e.Date)
                .HasColumnName("dtm_date")
                .IsRequired();

            // Unique constraint on StrName
            builder.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
