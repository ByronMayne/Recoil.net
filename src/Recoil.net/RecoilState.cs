using RecoilNet.State;
using System.ComponentModel;

namespace RecoilNet
{
	/// <summary>
	/// RecoilState is used to wrap an <see cref="Atom{T}"/> and
	/// the <see cref="IRecoilStore"/> instance that holds it's value.
	/// </summary>
	/// <typeparam name="T">The type of the value being held</typeparam>
	public abstract class RecoilState : INotifyPropertyChanged, IDisposable
	{
		private static readonly PropertyChangedEventArgs s_valueChangedEventArgs;

		/// <inheritdoc cref="INotifyPropertyChanged"/>
		public event PropertyChangedEventHandler? PropertyChanged;

		protected readonly SynchronizationContext? m_syncContext;
		protected IRecoilStore? m_store;

		/// <summary>
		/// Gets the recoil value that this state is watching 
		/// </summary>
		public RecoilValue RecoilValue { get; }

		static RecoilState()
		{
			s_valueChangedEventArgs = new PropertyChangedEventArgs("Value");
		}

		protected RecoilState(RecoilValue recoilValue, IRecoilStore? recoilStore)
		{
			m_syncContext = SynchronizationContext.Current;
			RecoilValue = recoilValue;
			PropertyChanged = null;
			SetStore(recoilStore);
		}

		/// <summary>
		/// Raised the event that the property has changed it's value.
		/// </summary>
		protected void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
			{
				return;
			}
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Invokes the delegate on the main thread
		/// </summary>
		protected void InvokeOnMain(Action action)
		{
			if (m_syncContext != null)
			{
				// We use send instead of 'Post' as we need to wait for the 
				// notifcation to be posted
				m_syncContext.Send(_ =>
				{
					action();
				}, null);
			}
			else
			{
				action();
			}
		}

		/// <summary>
		/// Invoked whenver the <see cref="IRecoilStore"/> instance 
		/// has changed.
		/// </summary>
		/// <param name="store">The current store if any</param>
		protected abstract void OnStoreSet(IRecoilStore? store);

		/// <summary>
		/// Raised whenever the store has received a new value for this state.
		/// </summary>
		/// <param name="store">The store that provided the value</param>
		/// <param name="newValue">The value that was provided</param>
		/// <returns>A task to await on</returns>
		protected abstract Task OnValueChangedAsync(IRecoilStore store, object? newValue);

		/// <summary>
		/// Raised whenever a value that this state depends on changes.
		/// </summary>
		/// <param name="store">The store that saw the change</param>
		/// <param name="dependentValue">The dependent value that changed</param>
		/// <returns>A task to await on</returns>
		protected abstract Task OnDependentChangedAsync(IRecoilStore store, RecoilValue dependentValue);

		/// <summary>
		/// Internal function to allow store to be set
		/// </summary>
		internal void SetStore(IRecoilStore? store)
		{
			OnStoreSet(store);
		}

		/// <summary>
		/// Internal function to invoke <see cref="OnValueChangedAsync(IRecoilStore, object?)"/>
		/// </summary>
		internal Task ValueChangedAsync(IRecoilStore recoilStore, object? newValue)
		{
			return OnValueChangedAsync(recoilStore, newValue);
		}

		/// <summary>
		/// Internal function to invoke <see cref="OnDependentChangedAsync(IRecoilStore, RecoilValue)"/>
		/// </summary>
		internal Task DependentChangedAsync(IRecoilStore recoilStore, RecoilValue dependentValue)
		{
			return OnDependentChangedAsync(recoilStore, dependentValue);
		}

		protected virtual void OnDispose()
		{
			PropertyChanged = null;
			m_store = null;

		}

		/// <summary>
		/// Disposes of this elements resources and cleans up all listeners
		/// </summary>
		public void Dispose()
		{
			OnDispose();
			GC.SuppressFinalize(this);
		}
	}
}
