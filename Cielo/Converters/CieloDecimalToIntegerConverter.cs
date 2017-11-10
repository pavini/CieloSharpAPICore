using System;
using Cielo.Core.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cielo.Core.Converters
{
    internal class CieloDecimalToIntegerConverter : JsonConverter
    {
        public override bool CanRead { get; } = true;

        public override bool CanWrite { get; } = true;

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            return NumberHelper.IntegerToDecimal(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var newValue = NumberHelper.DecimalToInteger(value);

                JToken.FromObject(newValue).WriteTo(writer);
            }
        }
    }
}
