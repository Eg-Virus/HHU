using System;
using System.ComponentModel;
using System.Reflection;

namespace AirObservationSystem.HHU.Core.Helpers
{
    public static class EnumUtil
    {
        /// <summary>
        /// To get the description value property of an enumeration type.
        /// NOTE: This method only works when dealing with enumerations that use the DescriptionAttribute.
        /// </summary>
        /// <param name="value">The enumeration type</param>
        /// <returns>Description attribute value.</returns>
        public static string StringValueOf(Enum value)
        {
            FieldInfo fi = value.GetType().GetRuntimeField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// To get the enumeration type value of by the description value string.
        /// NOTE: This method only works when dealing with enumerations that use the DescriptionAttribute.
        /// </summary>
        /// <param name="value">The description value of the enumeration type.</param>
        /// <param name="enumType">The type of enumeration</param>
        /// <returns>Enumeration type value.</returns>
        public static object EnumValueOf(string value, Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (StringValueOf((Enum)Enum.Parse(enumType, name)).Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }
            }

            throw new ArgumentException("The string is not a description or value of the specified enum.");
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <typeparam name="T">Base Type Enum</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The enum value matching the value provided.</returns>
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Returns the text value of the DescriptionAttribute for the given enum.
        /// If no description attribute is found, the enum's .ToString() value is returned.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        /// <returns>The value from the <see cref="DescriptionAttribute"/>. If not available then performs a ToString().</returns>
        public static string Description(this Enum enumeration)
        {
            var value = enumeration.ToString();
            var type = enumeration.GetType();
            var descAttribute = (DescriptionAttribute[])type.GetRuntimeField(value).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descAttribute.Length > 0 ? descAttribute[0].Description : value;
        }

        /// <summary>
        /// Parses the specified description value.
        /// </summary>
        /// <typeparam name="T">Base Type Enum</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The enum value matching the decription provided; otherwise the first item in your enum if not found.</returns>
        public static T ParseByDescription<T>(string value)
        {
            foreach (var name in Enum.GetNames(typeof(T)))
            {
                var enumValue = Parse<T>(name) as Enum;
                if (enumValue.Description().Equals(value))
                {
                    return Parse<T>(name);
                }
            }

            return default(T);
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.GetTypeInfo().IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetRuntimeFields())
            {
                var attribute = type.GetTypeInfo().GetCustomAttributes(typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }

        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        public static T Add<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"Could not append value from enumerated type '{typeof(T).Name}'.", ex);
            }
        }

        public static T Remove<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"Could not remove value from enumerated type '{typeof(T).Name}'.", ex);
            }
        }
    }
}
