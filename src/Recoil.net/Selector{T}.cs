using RecoilNet.State;
using RecoilNet.Utility;
using RecoilNet.Values;

namespace RecoilNet
{


    /// <summary>
    /// A selector represents a piece of derived state. You can think of 
    /// derived state as the output of passing state to a pure function 
    /// that modifies the given state in some way.
    /// </summary>
    public class Selector<T> : Primitive<T>
	{
		public delegate Task<T?> ValueGetter(PrimitiveEvaluator<T> getter);
		public delegate Task ValueSetter(IRecoilStore provider, T? Value);

		private readonly ValueGetter m_getter;
		private readonly ValueSetter? m_setter;

		/// <summary>
		/// Initializes a new instance of a selector that uses an sync method
		/// </summary>
		/// <param name="getter">The method to get the value</param>
		public Selector(string key, ValueGetter getter) : base(key, false)
		{
            Gaurd.NotNull(getter);
			m_getter = getter;
		}


		public Selector(string key, ValueGetter getter, ValueSetter setter) : base(key, true)
		{
            Gaurd.NotNull(getter);
            Gaurd.NotNull(getter);
			m_getter = getter;
			m_setter = setter;
		}

		/// <inheritdoc cref="Primitive"/>
		internal override string RenderDebug()
			=> $"Selector<{typeof(T).Name}>: {Key}";
	}
}
