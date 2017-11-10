using System;

namespace Cielo.Core.Configurations
{
    public class Merchant : IMerchant
    {
        public static readonly Merchant Sandbox = new Merchant(Guid.Parse("3361fcff-729a-4d0d-a8d2-dc8a1a380a66"), "SUNQYHBTXXWONSOUTGQGDQAKIVBHAIVXBEEZNFMN");

        public Merchant(Guid id, string key)
        {
            this.Id = id;
            this.Key = key;
        }

        public Guid Id { get; }

        public string Key { get; }
    }
}
