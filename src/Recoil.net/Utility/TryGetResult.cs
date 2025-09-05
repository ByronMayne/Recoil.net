using System.Diagnostics.CodeAnalysis;

namespace RecoilNet.Utility
{
    public struct TryGetResult<T>
    {
        /// <summary>
        /// The lue indicating whether the operation was successful.
        /// </summary>
        [MemberNotNullWhen(true, nameof(Value))]
        public bool Success { get; }

        /// <summary>
        /// The value retrieved, if <see cref="Success"/> is true; otherwise, null.
        /// </summary>
        public T? Value { get; }

        public TryGetResult(bool success, T? value)
        {
            Success = success;
            Value = value;
        }

        public static implicit operator bool(TryGetResult<T> result)
        {
            return result.Success;
        }

        public static implicit operator TryGetResult<T>(T value)
        {
            return new TryGetResult<T>(true, value);
        }

        public static TryGetResult<T> Failure => new TryGetResult<T>(false, default);
    }
}
