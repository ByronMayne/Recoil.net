using RecoilNet.State;
using System.ComponentModel;

namespace RecoilNet
{
	/// <summary>
	/// RecoilState is used to wrap an <see cref="Atom{T}"/> and
	/// the <see cref="IRecoilStore"/> instance that holds it's value.
	/// </summary>
	/// <typeparam name="T">The type of the value being held</typeparam>
	public sealed class RecoilState<T> : RecoilState
	{
		public class ValuePropertyDescriptor : PropertyDescriptor
		{
			public ValuePropertyDescriptor() : base("Value", null)
			{
				ComponentType = typeof(RecoilState<T>);
				IsReadOnly = false;
				PropertyType = typeof(T);
			}

			public override Type ComponentType { get; }
			public override bool IsReadOnly { get; }
			public override Type PropertyType { get; }


			public override bool CanResetValue(object component)
			{
				return true;
			}

			public override object? GetValue(object? component)
			{
				if (typeof(T) == typeof(string))
				{

					return "String Value";
				}
				return default(T);
			}

			public override void ResetValue(object component)
			{ }

			public override void SetValue(object? component, object? value)
			{ }

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
		}

		private const string VALUE_PROPERTY_NAME = "Value";

		private T? m_value;
		private RecoilValue<T> m_recoilValue;

		/// <summary>
		/// Gets or sets the value of th recoil state. It should be noted that setting the value 
		/// happens in the background in an async operation so the value will not be accessable right away.
		/// </summary>
		public T? Value
		{
			get => m_value;
			set
			{
				m_value = value; // set it for now but it will be overriden later 
				m_recoilValue.SetValue(m_store, value);
				IsLoading = true;
			}
		}

		/// <summary>
		/// Gets if the value is currently being calculated 
		/// </summary>
		public bool IsLoading { get; private set; }

		/// <summary>
		/// Creates a new Atom Accessor with the getter and setter defined.
		/// </summary>
		/// <param name="get">A delegate to fetch the value</param>
		/// <param name="set">A delegate to set the value</param>
		public RecoilState(RecoilValue<T> recoilValue, IRecoilStore? store) : base(recoilValue, store)
		{
			m_recoilValue = recoilValue;

			// Load the default value 
			Task.Run(async () =>
			{
				IsLoading = true;
				try
				{
					m_value = await m_recoilValue.GetValueAsync(store);
				}
				finally
				{
					IsLoading = false;
				}
				if (!EqualityComparer<T>.Default.Equals(m_value, default(T)))
				{
					RaisePropertyChanged(nameof(Value));
				}
			});
		}

		/// <inheritdoc cref="RecoilState"/>
		protected override void OnStoreSet(IRecoilStore? store)
		{
			if (m_store == store) return;

			if (m_store != null)
			{
				m_store.States.Remove(this);
			}

			m_store = store;

			if (m_store != null)
			{
				m_store.States.Add(this);
			}

			RaisePropertyChanged(nameof(Value));
		}

		/// <inheritdoc cref="RecoilState"/>
		protected override Task OnValueChangedAsync(IRecoilStore store, object? newValue)
		{
			m_value = (T?)newValue;
			RaisePropertyChanged(nameof(Value));
			return Task.CompletedTask;
		}

		/// <inheritdoc cref="RecoilState"/>
		protected override async Task OnDependentChangedAsync(IRecoilStore store, RecoilValue dependentValue)
		{
			m_value = await m_recoilValue.GetValueAsync(m_store);
			RaisePropertyChanged(nameof(Value));
		}
	}
}
