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
    [ValueObject<string>]
    public readonly partial struct Key
    {
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
    }
}
