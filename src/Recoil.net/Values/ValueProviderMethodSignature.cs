namespace RecoilNet.Values
{
    /// <summary>
    /// Specifies the method signature for the value provider.
    /// </summary>
    [Flags]
    public enum ValueProviderMethodSignature
    {
        /// <summary>
        /// No parameters.
        /// </summary>
        Empty = 0,
        /// <summary>
        /// Store parameter.
        /// </summary>
        Store = 1 << 1,
        /// <summary>
        /// Custom parameter.
        /// </summary>
        Parameter = 1 << 2,
    }
}
