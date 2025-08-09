using System.Runtime.CompilerServices;

namespace RecoilNet
{
    internal static class Gaurd
    {
        public static void ThrowIfNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string expression = "")
        {
            if (value is null)
            {
                throw new ArgumentNullException(expression, $"The value of {expression} cannot be null.");
            }
        }
    }
}
