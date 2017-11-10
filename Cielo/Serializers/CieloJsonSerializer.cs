using System.IO;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace Cielo.Core.Serializers
{

    internal class CieloJsonSerializer : ISerializer
    {
        protected Newtonsoft.Json.JsonSerializer Serializer { get; set; }

        public CieloJsonSerializer()
        {
            ContentType = "application/json";

            Serializer = Newtonsoft.Json.JsonSerializer.Create();

            Serializer.NullValueHandling = NullValueHandling.Ignore;
        }

        public CieloJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            ContentType = "application/json";
            Serializer = serializer;
        }

        public virtual string DateFormat { get; set; }

        public virtual string RootElement { get; set; }

        public virtual string Namespace { get; set; }

        public virtual string ContentType { get; set; }

        public virtual string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
#if DEBUG
                jsonTextWriter.Formatting = Formatting.Indented;
#endif

                Serializer.Serialize(jsonTextWriter, obj);

                return stringWriter.ToString();
            }
        }
    }
}
