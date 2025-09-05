using RecoilNet.Polyfils;
using System.Runtime.CompilerServices;

namespace RecoilNet
{
    internal static class Guard
    {
        public static void NotNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string expression = "")
        {
            if (value is null)
            {
                throw new ArgumentNullException(expression, $"The value of {expression} cannot be null.");
            }
        }

        public static void IsDefined<T>(T value) where T : Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                string error = $"The value '{value}' is not defined in the enum '{typeof(T).Name}'. The values provided are ";
                error += string.Join(", ", Enum.GetNames(typeof(T)));
                throw new ArgumentException(error, nameof(value));
            }
        }
    }
}
