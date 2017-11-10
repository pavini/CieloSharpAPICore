using Newtonsoft.Json.Converters;

namespace Cielo.Core.Converters
{
    internal class CreditCardExpirationDateConverter : IsoDateTimeConverter
    {
        public CreditCardExpirationDateConverter()
        {
            base.DateTimeFormat = "MM/yyyy";
        }
    }
}
