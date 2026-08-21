using ECommerceOrderManagement.Application.Common.Abstractions.Payments;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace ECommerceOrderManagement.Infrastructure.Payments.Iyzico;

internal sealed class IyzicoPaymentService(IOptions<IyzicoOptions> iyzicoOptions) : IPaymentService
{
    private readonly IyzicoOptions _iyzicoOptions = iyzicoOptions.Value;

    public string ProviderName => "Iyzico";

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var options = new Iyzipay.Options
        {
            ApiKey = _iyzicoOptions.ApiKey,
            SecretKey = _iyzicoOptions.SecretKey,
            BaseUrl = _iyzicoOptions.BaseUrl
        };

        var amount = request.Amount.ToString("0.00", CultureInfo.InvariantCulture);

        var nameParts = request.ShippingAddress.FullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        var buyerName = nameParts[0];

        var buyerSurname = nameParts.Length > 1
            ? nameParts[1]
            : nameParts[0];

        var iyzicoRequest = new CreatePaymentRequest
        {
            Locale = Locale.TR.ToString(),
            ConversationId = request.OrderId.ToString(),
            Price = amount,
            PaidPrice = amount,
            Currency = Currency.TRY.ToString(),
            Installment = 1,
            BasketId = request.OrderId.ToString(),
            PaymentChannel = PaymentChannel.WEB.ToString(),
            PaymentGroup = PaymentGroup.PRODUCT.ToString(),

            PaymentCard = new PaymentCard
            {
                CardHolderName = request.CardHolderName,
                CardNumber = request.CardNumber,
                ExpireMonth = request.ExpireMonth,
                ExpireYear = request.ExpireYear,
                Cvc = request.Cvc,
                RegisterCard = 0
            },

            Buyer = new Buyer
            {
                Id = request.CustomerId.ToString(),
                Name = buyerName,
                Surname = buyerSurname,
                GsmNumber = request.ShippingAddress.PhoneNumber,
                Email = "test@example.com",
                IdentityNumber = "74300864791",
                RegistrationAddress = request.ShippingAddress.AddressLine,
                Ip = "85.34.78.112",
                City = request.ShippingAddress.City,
                Country = request.ShippingAddress.Country,
                ZipCode = request.ShippingAddress.PostalCode
            },

            ShippingAddress = new Address
            {
                ContactName = request.ShippingAddress.FullName,
                City = request.ShippingAddress.City,
                Country = request.ShippingAddress.Country,
                Description = $"{request.ShippingAddress.AddressLine}, {request.ShippingAddress.District}",
                ZipCode = request.ShippingAddress.PostalCode
            },

            BillingAddress = new Address
            {
                ContactName = request.BillingAddress.FullName,
                City = request.BillingAddress.City,
                Country = request.BillingAddress.Country,
                Description = $"{request.BillingAddress.AddressLine}, {request.BillingAddress.District}",
                ZipCode = request.BillingAddress.PostalCode
            },

            BasketItems = request.Items.Select(item => new BasketItem
            {
                Id = item.ProductId.ToString(),
                Name = item.ProductName,
                Category1 = "ECommerce",
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = item.LineTotal.ToString("0.00", CultureInfo.InvariantCulture)
            })
            .ToList()
        };

        var iyzicoPayment = await Iyzipay.Model.Payment.Create(iyzicoRequest, options);

        if (iyzicoPayment.Status != Status.SUCCESS.ToString())
        {
            return new PaymentResult(false, null, iyzicoPayment.ErrorMessage ?? "Payment was rejected by iyzico.");
        }

        if (string.IsNullOrWhiteSpace(iyzicoPayment.PaymentId))
        {
            return new PaymentResult(false, null, "iyzico returned a successful response without a payment ID.");
        }

        return new PaymentResult(true, iyzicoPayment.PaymentId, null);
    }
}