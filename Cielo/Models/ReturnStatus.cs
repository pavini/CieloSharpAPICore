using System.Collections.Generic;
using Cielo.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cielo.Core.Models
{
    public class ReturnStatus
    {
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
        public string ProviderReturnCode { get; set; }
        public string ProviderReturnMessage { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Status? Status { get; set; }
        public List<Link> Links { get; set; }
    }
}
