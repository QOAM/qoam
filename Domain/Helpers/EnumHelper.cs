namespace RU.Uci.OAMarket.Domain.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumHelper
    {
        public static IEnumerable<TEnum> GetValues<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}