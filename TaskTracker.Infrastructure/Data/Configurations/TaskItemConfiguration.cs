using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;

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

            builder.Property(t => t.Priority)
                .IsRequired();

            builder.Property(t => t.DueDate)
                .IsRequired();

            // Define relationships
            builder.HasMany(t => t.Assignments)
                .WithOne(ta => ta.TaskItem)
                .HasForeignKey(ta => ta.TaskItemId);
        }
    }
}
