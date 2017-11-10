using System;
using Cielo.Core.Converters;
using Cielo.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cielo.Core.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
        }

        public CreditCard(Guid cardToken, string securityCode, CardBrand brand)
        {
            this.CardToken = cardToken;
            this.SecurityCode = securityCode;
            this.Brand = brand;
        }

        public CreditCard(string cardNumber, string holder, DateTime expirationDate, string securityCode, CardBrand brand, bool saveCard = false)
        {
            this.CardNumber = cardNumber;
            this.Holder = holder;
            this.ExpirationDate = expirationDate;
            this.SecurityCode = securityCode;
            this.Brand = brand;
            this.SaveCard = saveCard;
        }

        public string CardNumber { get; set; }
        public Guid? CardToken { get; set; }
        public string Holder { get; set; }
        [JsonConverter(typeof(CreditCardExpirationDateConverter))]
        public DateTime? ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public bool? SaveCard { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public CardBrand? Brand { get; set; }
    }
}
