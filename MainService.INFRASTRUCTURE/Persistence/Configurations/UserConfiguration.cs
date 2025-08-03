using MainService.CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainService.INFRASTRUCTURE.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("tbl_users");
            // Primary key configuration
            builder.HasKey(u => u.Id);  // Assuming `Id` is the primary key.

            // Map properties to database columns with required constraints
            builder.Property(u => u.Username)
                .HasColumnName("str_user_name")
                .IsRequired();  // Ensuring this field is not null

            builder.Property(u => u.Password)
                .HasColumnName("str_password")
                .IsRequired();  // Ensuring this field is not null

            // Configure a unique index on the Username column
            builder.HasIndex(u => u.Username)
                .IsUnique();  // Ensure that the username is unique in the database
        }
    }
}