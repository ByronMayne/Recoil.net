namespace RecoilNet.Effects
{
    public interface IPrimitiveEffect<T>
    {
        /// <summary>
        /// Invoked whenever the value of the atom is set
        /// </summary>
        /// <param name="newValue">The new value being set</param>
        /// <param name="oldValue">The previous value</param>
        /// <param name="isReset">If the value was a reset or not</param>
        void OnSet(T? newValue, T? oldValue, bool isReset);
    }
}
