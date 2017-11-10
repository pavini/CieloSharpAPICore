using System;

namespace Cielo.Core.Helper
{
    internal static class NumberHelper
    {
        public static decimal IntegerToDecimal(object value)
        {
            return Convert.ToDecimal(value) / 100;
        }

        public static object DecimalToInteger(object value)
        {
            return Convert.ToInt32(Convert.ToDecimal(value) * 100);
        }
    }
}
