using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CORE.Entities;

namespace INFRASTRUCTURE.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            // Table name mapping
            builder.ToTable("tbl_trns");

            // Primary key mapping
            builder.HasKey(t => t.Id);

            // Column mappings
            builder.Property(t => t.Id)
                .HasColumnName("str_id")
                .HasMaxLength(20)  // character varying(20)
                .IsRequired();

            builder.Property(t => t.TrnType)
                .HasColumnName("str_trn_type")
                .HasMaxLength(20)  // character varying(20)
                .IsRequired(false);  // Allowing null for optional fields

            builder.Property(t => t.TrnCat)
                .HasColumnName("str_trn_cat")
                .HasMaxLength(20)  // character varying(20)
                .IsRequired(false);  // Allowing null for optional fields

            builder.Property(t => t.Amount)
                .HasColumnName("int_amount")
                .HasColumnType("numeric")  // Numeric type in PostgreSQL
                .IsRequired(false);  // Amount can be null

            builder.Property(t => t.Date)
                .HasColumnName("dtm_date")
                .HasColumnType("timestamp without time zone") // Timestamp without time zone
                .IsRequired(false);  // Date can be null

            builder.Property(t => t.Reason)
                .HasColumnName("str_reason")
                .HasMaxLength(200)  // character varying(200)
                .IsRequired(false);  // Reason is optional

            builder.Property(t => t.User)
                .HasColumnName("str_user")
                .HasMaxLength(20)  // character varying(20)
                .IsRequired(false);  // User can be null

            builder.Property(t => t.Account)
                .HasColumnName("str_account")
                .HasMaxLength(20)  // character varying(20)
                .IsRequired(false);  // Account can be null

            // Optionally, you can add an index if necessary
            builder.HasIndex(t => t.Id).IsUnique();
        }
    }
}
