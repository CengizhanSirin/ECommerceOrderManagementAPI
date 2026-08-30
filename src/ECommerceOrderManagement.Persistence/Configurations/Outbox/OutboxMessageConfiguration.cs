using ECommerceOrderManagement.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Outbox;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages", "outbox");

        builder.HasKey(message => message.Id);

        builder.Property(message => message.Id)
            .ValueGeneratedNever();

        builder.Property(message => message.Type)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(message => message.Payload)
            .IsRequired();

        builder.Property(message => message.OccurredOnUtc)
            .IsRequired();

        builder.Property(message => message.ProcessedOnUtc);

        builder.Property(message => message.RetryCount)
            .IsRequired();

        builder.Property(message => message.LastError)
            .HasMaxLength(2000);

        builder.HasIndex(message => new
        {
            message.ProcessedOnUtc,
            message.RetryCount,
            message.OccurredOnUtc
        });
    }
}