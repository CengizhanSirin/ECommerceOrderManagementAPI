using ECommerceOrderManagement.Domain.Addresses;
using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Domain.Common;
using ECommerceOrderManagement.Domain.Coupons;
using ECommerceOrderManagement.Domain.Inventory;
using ECommerceOrderManagement.Domain.Orders;
using ECommerceOrderManagement.Domain.Payments;
using ECommerceOrderManagement.Domain.ShoppingCarts;
using ECommerceOrderManagement.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ECommerceOrderManagement.Persistence.Contexts;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<UserAddressPreference> UserAddressPreferences => Set<UserAddressPreference>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddDomainEventsToOutboxMessages();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AddDomainEventsToOutboxMessages()
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entry => entry.Entity.DomainEvents.Count > 0)
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(aggregateRoot => aggregateRoot.DomainEvents)
            .ToList();

        if (domainEvents.Count == 0)
        {
            return;
        }

        var outboxMessages = domainEvents
            .Select(domainEvent =>
            new OutboxMessage(
                domainEvent.GetType().Name,
                JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                domainEvent.OccurredOnUtc))
            .ToList();

        OutboxMessages.AddRange(outboxMessages);

        foreach (var aggregateRoot in aggregateRoots)
        {
            aggregateRoot.ClearDomainEvents();
        }
    }
}