using RecoilNet.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilNet.Values
{
    /// <summary>
    /// Contains factory methods for creating <see cref="ValueProvider{T}"/> instances.
    /// </summary>
    public static class ValueProvider
    {
        /// <summary>
        /// Creates a value provider that returns a constant value.
        /// </summary>
        /// <param name="constant">The constant value.</param>
        /// <returns>A new <see cref="ValueProvider{T}"/> instance.</returns>
        public static ValueProvider<T> Create<T>(T? constant)
        {
            return new ValueProvider<T>(
                evaluation: () => constant,
                type: ValueProviderType.Constant,
                signature: ValueProviderMethodSignature.Empty);
        }

        /// <summary>
        /// Creates a value provider that uses a factory method.
        /// </summary>
        /// <param name="factory">The factory method.</param>
        /// <returns>A new <see cref="ValueProvider{T}"/> instance.</returns>
        public static ValueProvider<T> Create<T>(Func<T> factory)
        {
            return new ValueProvider<T>(
                evaluation: factory,
                type: ValueProviderType.Factory,
                signature: ValueProviderMethodSignature.Empty);
        }

        /// <summary>
        /// Creates a value provider that returns an asynchronous value.
        /// </summary>
        /// <param name="asyncValue">The asynchronous value.</param>
        /// <returns>A new <see cref="ValueProvider{T}"/> instance.</returns>
        public static ValueProvider<T> Create<T>(Task<T> asyncValue)
        {
            return new ValueProvider<T>(
                evaluation: () => asyncValue,
                type: ValueProviderType.AsyncValue,
                signature: ValueProviderMethodSignature.Empty);
        }

        /// <summary>
        /// Creates a value provider that uses an asynchronous factory method.
        /// </summary>
        /// <param name="asyncFactory">The asynchronous factory method.</param>
        /// <returns>A new <see cref="ValueProvider{T}"/> instance.</returns>
        public static ValueProvider<T> Create<T>(Func<Task<T>> asyncFactory)
        {
            return new ValueProvider<T>(
                evaluation: asyncFactory,
                type: ValueProviderType.AsyncValue,
                signature: ValueProviderMethodSignature.Empty);
        }

        /// <summary>
        /// Creates a value provider that uses a recoil value factory method.
        /// </summary>
        /// <param name="recoilValue">The recoil value factory method.</param>
        /// <returns>A new <see cref="ValueProvider{T}"/> instance.</returns>
        public static ValueProvider<T> Create<T>(Func<IRecoilStore, T> recoilValue)
        {
            return new ValueProvider<T>(
                evaluation: recoilValue,
                type: ValueProviderType.AtomValue,
                signature: ValueProviderMethodSignature.Store);
        }

        /// <summary>
        /// Creates a value provider that uses an asynchronous recoil value factory method.
        /// </summary>
        /// <param name="asyncRecoilValue">The asynchronous recoil value factory method.</param>
        /// <returns>A new <see cref="ValueProvider{T}"/> instance.</returns>
        public static ValueProvider<T> Create<T>(Func<IRecoilStore, Task<T>> asyncRecoilValue)
        {
            return new ValueProvider<T>(
                evaluation: asyncRecoilValue,
                type: ValueProviderType.AtomValue,
                signature: ValueProviderMethodSignature.Store);
        }

        /// <summary>
        /// Creates a value provider that fetches the value from an atom.
        /// </summary>
        /// <param name="atomValue">The atom to fetch the value from.</param>
        /// <returns>A new <see cref="ValueProvider{T}"/> instance.</returns>
        public static ValueProvider<T> Create<T>(Atom<T> atomValue)
        {
            return new ValueProvider<T>(
                evaluation: (IRecoilStore store) => store.GetAsync(atomValue, CancellationToken.None),
                type: ValueProviderType.AtomValue,
                signature: ValueProviderMethodSignature.Store);
        }

        /// <summary>
        /// Creates a value provider that fetches the value from a selector.
        /// </summary>
        /// <param name="selector">The selector to fetch the value from.</param>
        /// <returns>A new <see cref="ValueProvider{T}"/> instance.</returns>
        public static ValueProvider<T> Create<T>(Selector<T> selector)
        {
            return new ValueProvider<T>(
                evaluation: (IRecoilStore store) => store.GetAsync(selector, CancellationToken.None),
                type: ValueProviderType.AtomValue,
                signature: ValueProviderMethodSignature.Store);
        }

        public static ValueProvider<T> Create<T, TParam>(Func<TParam, T?> keyedFactory) where TParam : notnull
        {
            return new ValueProvider<T>(
                evaluation: keyedFactory,
                type: ValueProviderType.Factory,
                signature: ValueProviderMethodSignature.Parameter);
        }

        public static ValueProvider<T> Create<T, TParam>(AtomFamily<T, TParam> atomFamily) where TParam : notnull
        {
            return new ValueProvider<T>(
                evaluation: (IRecoilStore store, TParam param) => store.GetAsync(atomFamily(param)),
                type: ValueProviderType.AtomValue,
                signature: ValueProviderMethodSignature.Store | ValueProviderMethodSignature.Parameter);
        }

        public static ValueProvider<T> Create<T, TParam>(AtomFamily<T, TParam> atomFamily, TParam parameter) where TParam : notnull
        {
            return new ValueProvider<T>(
                evaluation: (IRecoilStore store) => store.GetAsync(atomFamily(parameter)),
                type: ValueProviderType.AtomValue,
                signature: ValueProviderMethodSignature.Store);
        }
    }
}
