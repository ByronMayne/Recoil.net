using RecoilNet.Converters;
using RecoilNet.State;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace RecoilNet
{
	/// <summary>
	/// RecoilState is used to wrap an <see cref="Atom{T}"/> and
	/// the <see cref="IRecoilStore"/> instance that holds it's value.
	/// </summary>
	/// <typeparam name="T">The type of the value being held</typeparam>
	[TypeConverter(typeof(RecoilStateConverter))]
	public abstract class RecoilState : INotifyPropertyChanged
	{
		private static PropertyChangedEventArgs s_valueChangedEventArgs;

		/// <inheritdoc cref="INotifyPropertyChanged"/>
		public event PropertyChangedEventHandler? PropertyChanged;

		protected IRecoilStore? m_store;

		static RecoilState()
		{
			s_valueChangedEventArgs = new PropertyChangedEventArgs("Value");
		}

		protected RecoilState(RecoilValue recoilObject, IRecoilStore? recoilStore)
		{
			PropertyChanged = null;
			SetStore(recoilStore); 
		}

		/// <summary>
		/// Raised the event that the property has changed it's value.
		/// </summary>
		protected void RaisePropertyChanged(string propertyName)
		{
			if(PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		/// <summary>
		/// Sets the instance of the recoil store that we are using.
		/// </summary>
		internal void SetStore(IRecoilStore? store)
		{
			if (m_store == store) return;

			if (m_store != null)
			{
				m_store.OnValueChanged -= OnValuesChanged;
			}

			m_store = store;

			if(m_store != null)
			{
				m_store.OnValueChanged += OnValuesChanged;
			}

			RaisePropertyChanged("Value");
		}

		/// <summary>
		/// Invoked whenver a value in the store changes
		/// </summary>
		protected abstract void OnValuesChanged(IList<RecoilValue> changedValues);

		/// <summary>
		/// Returns back the value of the state
		/// </summary>
		public abstract object? GetValue();
	}
}
