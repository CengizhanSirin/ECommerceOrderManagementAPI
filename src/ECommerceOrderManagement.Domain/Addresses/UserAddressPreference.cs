using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Addresses;

public sealed class UserAddressPreference : AggregateRoot
{
    public Guid UserId { get; private set; }

    public Guid? DefaultShippingAddressId { get; private set; }

    public Guid? DefaultBillingAddressId { get; private set; }

    private UserAddressPreference()
    {
    }

    private UserAddressPreference(Guid id, Guid userId) : base(id)
    {
        UserId = userId;
    }

    public static UserAddressPreference Create(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User identifier cannot be empty.", nameof(userId));
        }

        return new UserAddressPreference(Guid.NewGuid(), userId);
    }

    public void SetDefaultShippingAddress(Guid addressId)
    {
        if (addressId == Guid.Empty)
        {
            throw new ArgumentException("Address identifier cannot be empty.", nameof(addressId));
        }

        DefaultShippingAddressId = addressId;
    }

    public void SetDefaultBillingAddress(Guid addressId)
    {
        if (addressId == Guid.Empty)
        {
            throw new ArgumentException("Address identifier cannot be empty.", nameof(addressId));
        }

        DefaultBillingAddressId = addressId;
    }

    public void ClearAddress(Guid addressId)
    {
        if (DefaultShippingAddressId == addressId)
        {
            DefaultShippingAddressId = null;
        }

        if (DefaultBillingAddressId == addressId)
        {
            DefaultBillingAddressId = null;
        }
    }
}