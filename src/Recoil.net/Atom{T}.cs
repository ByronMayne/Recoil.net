﻿using RecoilNet.State;
using System.Diagnostics;
using System.Windows.Input;

namespace RecoilNet
{
	/// <summary>
	/// Atoms are units of state. They're updateable and subscribable: 
	/// when an atom is updated, each subscribed component is re-rendered with the new value. 
	/// </summary>
	[DebuggerDisplay("Atom<{typeof(T).Name}>: {Key}")]
	public class Atom<T> : RecoilValue<T>
	{
		private readonly Func<IRecoilStore?, T?> m_defaultValueProvider;

		/// <summary>
		/// Initializes a new instance of an Atom using whatever the default value is
		/// </summary>
		/// <param name="key"></param>
		internal Atom(string key) : this(key, default(T))
		{}

		/// <summary>
		/// Initializes a new atom using a set defalut value
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		internal Atom(string key, T? defaultValue) : base(key, true)
		{
			// Hard coded value
			m_defaultValueProvider = (_) => defaultValue;
		}

		/// <inheritdoc cref="RecoilValue{T}"/>
		internal Atom(string key, Atom<T> atom) : base(key, true)
		{
			ArgumentNullException.ThrowIfNull(atom);
			m_defaultValueProvider = atom.GetValue;
			m_dependents.Add(atom);
		}

		/// <inheritdoc cref="RecoilValue{T}"/>
		internal Atom(string key, Selector<T> defaultValue) : base(key, true)
		{
			ArgumentNullException.ThrowIfNull(defaultValue);
			m_defaultValueProvider = defaultValue.GetValue;
			m_dependents.Add(defaultValue);
		}

		/// <summary>
		/// Gets the value of a <see cref="RecoilValue"/> from the instance of the store. 
		/// </summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <param name="recoilStore">The store instance to attempt to fetch it from</param>
		/// <returns>The value</returns>
		public override T? GetValue(IRecoilStore? recoilStore)
		{
			if (recoilStore == null || !recoilStore.TryGetValue<T>(this, out T? storeValue))
			{
				return m_defaultValueProvider(recoilStore);
			}
			return storeValue;
		}
	}
}