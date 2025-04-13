using System;
using System.ComponentModel;
using System.Reflection;

namespace Tuxedo.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}