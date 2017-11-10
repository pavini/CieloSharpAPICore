using System;

namespace Cielo.Core.Configurations
{
    public interface IMerchant
    {
        Guid Id { get; }
        string Key { get; }
    }
}
