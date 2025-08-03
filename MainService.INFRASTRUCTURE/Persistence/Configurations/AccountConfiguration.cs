// INFRASTRUCTURE/Persistence/Configurations/AccountConfiguration.cs
using MainService.CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainService.INFRASTRUCTURE.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Define the table name (optional as EF Core will use plural by default)
            builder.ToTable("tbl_refmas_accounts");

            // Primary key configuration
            builder.HasKey(a => a.Id);  // Primary key: str_id

            // Column mappings
            builder.Property(a => a.Id)
                .HasColumnName("str_id")
                .IsRequired()
                .HasMaxLength(20);  // Set max length for str_id

            builder.Property(a => a.Name)
                .HasColumnName("str_name")
                .IsRequired()
                .HasMaxLength(100);  // Set max length for str_name

            builder.Property(a => a.Active)
                .HasColumnName("str_active")
                .IsRequired()
                .HasMaxLength(1);  // str_active should have a length of 1 character

            builder.Property(a => a.User)
                .HasColumnName("str_user")
                .IsRequired()
                .HasMaxLength(20);  // str_user length 20

            builder.Property(a => a.Date)
                .HasColumnName("dtm_date")
                .IsRequired()
                .HasColumnType("timestamp without time zone");  // Use timestamp without time zone

            builder.Property(a => a.IsMain)
                .HasColumnName("str_ismain")
                .HasDefaultValue('N') // Default value 'N' for StrIsMain column
                .HasMaxLength(1);  // str_ismain length 1

            // Unique constraint on str_name column
            builder.HasIndex(a => a.Name)
                .IsUnique();  // Ensure str_name is unique
        }
    }
}
