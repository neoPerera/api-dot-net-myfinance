using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MainService.CORE.Entities;

namespace MainService.INFRASTRUCTURE.Persistence.Configurations
{
    public class IncomeConfiguration : IEntityTypeConfiguration<Income>
    {
        public void Configure(EntityTypeBuilder<Income> builder)
        {
            // Table name mapping
            builder.ToTable("tbl_refmas_income");

            // Primary key mapping
            builder.HasKey(i => i.Id);

            // Column mappings
            builder.Property(i => i.Id)
                .HasColumnName("str_id")
                .HasMaxLength(20)  // varchar(20)
                .IsRequired();

            builder.Property(i => i.Name)
                .HasColumnName("str_name")
                .HasMaxLength(100) // varchar(100)
                .IsRequired();

            builder.Property(i => i.Active)
                .HasColumnName("str_active")
                .HasMaxLength(1)   // char(1)
                .IsRequired();

            builder.Property(i => i.Date)
                .HasColumnName("dtm_date")
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            // Unique constraint for str_name column
            builder.HasIndex(i => i.Name)
                .IsUnique();
        }
    }
}
