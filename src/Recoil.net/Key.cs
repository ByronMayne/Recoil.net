using RecoilNet.Utility;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using Vogen;

namespace RecoilNet
{
    /// <summary>
    /// Represents a unique string identifier for a value in Recoil.
    /// <para>
    /// The <see cref="Key"/> must be unique across the entire application.
    /// It is used to identify and reference values within the Recoil state management system.
    /// </para>
    /// </summary>
    [ValueObject<string>(conversions: Conversions.TypeConverter)]
    public readonly partial struct Key
    {
        /// <summary>
        /// Creates a new <see cref="Key"/> instance from the provided string value.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="value">The value itself</param>
        /// <returns>The created key</returns>
        public static Key CreateChildKey<T>(Key parent, T value)
        {   
            return From($"{parent}.{value}");
        }

        /// <summary>
        /// Normalizes the input string by removing whitespace, converting letters to lowercase,
        /// and retaining only digits, '-', '=', and ':' characters.
        /// </summary>
        /// <param name="input">The input string to normalize.</param>
        /// <returns>A normalized string suitable for use as a <see cref="Key"/>.</returns>
        private static string NormalizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                if (char.IsDigit(c) || c == '-' || c == '=' || c == ':')
                {
                    builder.Append(c);
                }
                else if (char.IsLetter(c))
                {
                    builder.Append(char.ToLowerInvariant(c));
                }
                // skip all other characters
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates a unique ery key based on the given expression.
        /// </summary>
        public static Key From<T>(Expression<T> expression)
        {
            string path = ExpressionUtility.GetPropertyPath(expression);
            return From(path);
        }
    }
}
