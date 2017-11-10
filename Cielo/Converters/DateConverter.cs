using Newtonsoft.Json.Converters;

namespace Cielo.Core.Converters
{
    internal class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            this.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
