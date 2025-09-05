using RecoilNet.Diagnostics;
using RecoilNet.Effects;
using RecoilNet.Values;
using System.Runtime.CompilerServices;

namespace RecoilNet
{

	/// <summary>
	/// Atoms are units of state. They're updateable and subscribe: 
	/// when an atom is updated, each subscribed component is re-rendered with the new value. 
	/// </summary>

	public class Atom<T> : Primitive<T>
	{
		private readonly ValueProvider<T> m_defaultValueProvider;

		/// <summary>
		/// Gets the list of effects that are applied to the atom
		/// </summary>
		public IReadOnlyList<IPrimitiveEffect<T>> Effects { get; }

		public ValueProvider<T> DefaultValue => m_defaultValueProvider;

        internal Atom(
			Key key, 
			CallerInfo creatorInfo,
			ValueProvider<T> defaultValueProvider, 
			IPrimitiveEffect<T>[] effects) : base(key,  creatorInfo, true)
		{
            Guard.NotNull(defaultValueProvider);
			Guard.NotNull(effects, nameof(effects));

            Effects = effects ?? Array.Empty<IPrimitiveEffect<T>>();
			m_defaultValueProvider = defaultValueProvider;
		}


		/// <inheritdoc cref="Primitive"/>
		internal override string RenderDebug()
				=> $"Atom<{typeof(T).Name}>: {Key}";


	}
}