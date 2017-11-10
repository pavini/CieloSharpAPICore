using System;
using Cielo.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cielo.Core.Models
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
        public string Identity { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public IdentityType? IdentityType { get; set; }
        public string Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public Address Address { get; set; }
        public Address DeliveryAddress { get; set; }
    }
}
