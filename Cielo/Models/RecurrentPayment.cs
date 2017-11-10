using System;
using Cielo.Core.Converters;
using Cielo.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cielo.Core.Models
{
    public class RecurrentPayment 
    {
        public RecurrentPayment()
        {
        }

        public RecurrentPayment(Interval interval, DateTime endDate)
        {
            this.Interval = interval;
            this.EndDate = endDate;
            this.AuthorizeNow = true;
        }

        public RecurrentPayment(Interval interval, DateTime startDate, DateTime endDate)
        {
            if (startDate.Date <= DateTime.Now.Date)
            {
                throw new ArgumentException("startDate: the starting date must be in the future");
            }

            this.Interval = interval;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.AuthorizeNow = false;
        }

        public Guid? RecurrentPaymentId { get; set; }
        public bool? AuthorizeNow { get; set; }
        [JsonConverter(typeof(DateConverter))]
        public DateTime? StartDate { get; set; }
        [JsonConverter(typeof(DateConverter))]
        public DateTime? EndDate { get; set; }
        [JsonConverter(typeof(DateConverter))]
        public DateTime? NextRecurrency { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Interval Interval { get; set; }
        public Link Link { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonMessage { get; set; }
    }
}
