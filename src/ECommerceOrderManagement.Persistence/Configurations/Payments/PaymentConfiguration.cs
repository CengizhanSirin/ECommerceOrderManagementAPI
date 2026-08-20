using ECommerceOrderManagement.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Payments;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", "payment");

        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Id)
            .ValueGeneratedNever();

        builder.Property(payment => payment.OrderId)
            .IsRequired();

        builder.Property(payment => payment.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(payment => payment.Status)
            .IsRequired();

        builder.Property(payment => payment.Method)
            .IsRequired();

        builder.Property(payment => payment.Provider)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(payment => payment.ProviderPaymentId)
            .HasMaxLength(100);

        builder.Property(payment => payment.FailureReason)
           .HasMaxLength(500);

        builder.Property(payment => payment.CreatedAtUtc)
            .IsRequired();

        builder.Property(payment => payment.UpdatedAtUtc);

        builder.HasIndex(payment => payment.OrderId);

        builder.HasIndex(payment => payment.ProviderPaymentId)
            .IsUnique()
            .HasFilter("[ProviderPaymentId] IS NOT NULL");

        builder.Ignore(payment => payment.DomainEvents);
    }
}