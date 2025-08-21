namespace RecoilNet.Values
{
    /// <summary>
    /// Specifies the type of value provider.
    /// </summary>
    public enum ValueProviderType
    {
        /// <summary>
        /// Default value provider.
        /// </summary>
        Default,
        /// <summary>
        /// Constant value provider.
        /// </summary>
        Constant,
        /// <summary>
        /// Factory value provider.
        /// </summary>
        Factory,
        /// <summary>
        /// Asynchronous value provider.
        /// </summary>
        AsyncValue,
        /// <summary>
        /// Asynchronous factory value provider.
        /// </summary>
        AsyncFactory,
        /// <summary>
        /// Atom value provider.
        /// </summary>
        AtomValue,
        /// <summary>
        /// Asynchronous recoil value provider.
        /// </summary>
        AsyncRecoilValue,
        /// <summary>
        /// Selector value provider.
        /// </summary>
        SelectorValue,



        keyedFactory
    }
}
