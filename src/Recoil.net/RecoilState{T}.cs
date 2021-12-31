using RecoilNet.State;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace RecoilNet
{
	/// <summary>
	/// RecoilState is used to wrap an <see cref="Atom{T}"/> and
	/// the <see cref="IRecoilStore"/> instance that holds it's value.
	/// </summary>
	/// <typeparam name="T">The type of the value being held</typeparam>
	public sealed class RecoilState<T> : RecoilState
	{
		private readonly RecoilValue<T> m_recoilObject;
		public delegate T? GetDelegate();
		public delegate void SetDelegate(T? value);

		/// <summary>
		/// Gets the value of the delegate 
		/// </summary>
		public GetDelegate Getter { get; }

		/// <summary>
		/// Sets the value of the delegate
		/// </summary>
		public SetDelegate Setter { get; }

		/// <summary>
		/// Gets the current value of the state
		/// </summary>
		public T? Value
		{
			get => m_recoilObject.GetValue(m_store);
			set => m_recoilObject.SetValue(m_store, value);
		}

		/// <summary>
		/// Creates a new Atom Accessor with the getter and setter defined.
		/// </summary>
		/// <param name="get">A delegate to fetch the value</param>
		/// <param name="set">A delegate to set the value</param>
		public RecoilState(RecoilValue<T> recoilObject, IRecoilStore? store) : base(recoilObject, store)
		{
			m_recoilObject = recoilObject;
			Getter = () => Value;
			Setter = (v) => Value = v;
		}


		/// <inheritdoc cref="RecoilState"/>
		public override object? GetValue()
			=> Value;

		/// <inheritdoc cref="RecoilState"/>
		protected override void OnValuesChanged(IList<RecoilValue> changedValues)
		{
			if (changedValues.Contains(m_recoilObject))
			{
				RaisePropertyChanged(nameof(Value));
			}
		}

		/// <summary>
		/// Deconstructors
		/// </summary>
		/// <param name="get"></param>
		/// <param name="set"></param>
		public void Deconstruct(out GetDelegate get, out SetDelegate set)
		{
			get = Getter;
			set = Setter;
		}
	}
}
