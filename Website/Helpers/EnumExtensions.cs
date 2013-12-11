namespace RU.Uci.OAMarket.Website.Helpers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public static class EnumExtensions
    {
        public static string GetName<TEnum>(this TEnum value)
        {
            var displayAttribute = GetDisplayAttribute(value);

            return displayAttribute == null ? value.ToString() : displayAttribute.GetName();
        }

        private static DisplayAttribute GetDisplayAttribute<TEnum>(TEnum value)
        {
            return value.GetType()
                        .GetField(value.ToString())
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault();
        }

        public static object GetValue<TEnum>(this TEnum value)
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));

            return Convert.ChangeType(value, underlyingType);
        }
    }
}