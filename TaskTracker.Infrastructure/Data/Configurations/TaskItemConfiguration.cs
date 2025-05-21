using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Infrastructure.Data.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("TaskItems");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.Description)
                   .HasMaxLength(500);

            builder.Property(t => t.DueDate)
                   .IsRequired();

            builder.Property(t => t.Priority)
                   .IsRequired();

            // Configure owned value object: Category
            builder.OwnsOne(t => t.Category, category =>
            {
                category.Property(c => c.Name)
                        .IsRequired()
                        .HasColumnName("Category")
                        .HasMaxLength(50);
            });

            // Configure optional value object: Recurrence
            builder.OwnsOne(t => t.Recurrence, recurrence =>
            {
                recurrence.Property(r => r.Interval)
                         .HasColumnName("RecurrenceInterval");

                recurrence.Property(r => r.Unit)
                         .HasColumnName("RecurrenceUnit");
            });
        }
    }
}