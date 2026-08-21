using ECommerceOrderManagement.Application.Common.Abstractions.Payments;
using Iyzipay;
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
                Id = request.OrderId.ToString(),
                Name = "Test",
                Surname = "Customer",
                GsmNumber = "+905350000000",
                Email = "test@example.com",
                IdentityNumber = "74300864791",
                RegistrationAddress = "Istanbul",
                Ip = "85.34.78.112",
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34732"
            },

            ShippingAddress = new Address
            {
                ContactName = request.CardHolderName,
                City = "Istanbul",
                Country = "Turkey",
                Description = "Sandbox test address",
                ZipCode = "34732"
            },

            BillingAddress = new Address
            {
                ContactName = request.CardHolderName,
                City = "Istanbul",
                Country = "Turkey",
                Description = "Sandbox test address",
                ZipCode = "34732"
            },

            BasketItems =
            [
                new BasketItem
                {
                    Id = request.OrderId.ToString(),
                    Name = "Test Product",
                    Category1 = "ECommerce",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = amount
                }
            ]
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