using MainService.CORE.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainService.INFRASTRUCTURE.Persistence.Configurations
{
    public class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            builder.ToTable("tbl_forms");
            // Primary key configuration
            builder.HasKey(f => f.FormId);

            // Map properties to database columns with required constraints
            builder.Property(f => f.FormId)
                .HasColumnName("str_form_id")
                .IsRequired();  // Ensuring this field is not null

            builder.Property(f => f.FormName)
                .HasColumnName("str_form_name")
                .IsRequired();  // Ensuring this field is not null

            builder.Property(f => f.MenuId)
                .HasColumnName("str_menu_id");

            builder.Property(f => f.Icon)
                .HasColumnName("str_icon");

            builder.Property(f => f.Link)
                .HasColumnName("str_link")
                .IsRequired();  // Ensuring this field is not null

            builder.Property(f => f.Component)
                .HasColumnName("str_component")
                .IsRequired();  // Ensuring this field is not null

            builder.Property(f => f.IsMenu)
                .HasColumnName("str_isMenu")
                .IsRequired();  // Ensuring this field is not null

            builder.Property(f => f.Active)
                .HasColumnName("str_active")
                .IsRequired();  // Ensuring this field is not null

            // Define unique index if necessary
            builder.HasIndex(f => f.FormId)
                .IsUnique();  // Ensure the form ID is unique in the database
        }
    }
}