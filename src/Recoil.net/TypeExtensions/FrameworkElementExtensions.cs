using Recoil.net.Utility;
using RecoilNet.Internal;
using RecoilNet.State;
using RecoilNet.Utility;
using System.Reflection;
using System.Windows;

namespace RecoilNet
{
	/// <summary>
	/// Contains helper methods for working with Framework Extensions
	/// </summary>
	public static class FrameworkElementExtensions
	{
		/// <summary>
		/// Invokes an action right away on a framework element if it's loaded otherwise will invoke after it's loaded
		/// </summary>
		public static void InvokeWhenLoaded(this FrameworkElement element, Action action)
		{
			if (element.IsLoaded)
			{
				action();
			}
			else
			{
				RoutedEventHandler loadedEventListener = (object sender, RoutedEventArgs args) =>
				{
					action();
				};
				element.Loaded += loadedEventListener;
			}
		}

		/// <summary>
		/// Initalizes a new <see cref="RecoilState{T}"/> object by binding to an <see cref="Atom{T}"/>
		/// </summary>
		/// <typeparam name="T">The value type of the atom</typeparam>
		/// <param name="element">The element to bind it too</param>
		/// <param name="atom">The atom that points at the value</param>
		/// <param name="stateOptions">The options to allow you to configure the options</param>
		/// <returns>The value holder</returns>
		public static RecoilState<T> UseRecoilState<T>(this FrameworkElement element, Atom<T> atom, RecoilStateOptions stateOptions = RecoilStateOptions.Default)
		{
			return UseRecoilStateInternal<T>(element, atom, stateOptions);
		}

		/// <summary>
		/// Initalizes a new <see cref="RecoilState{T}"/> object by binding to an <see cref="Selector{T}"/>
		/// </summary>
		/// <typeparam name="T">The value type of the atom</typeparam>
		/// <param name="element">The element to bind it too</param>
		/// <param name="atom">The atom that points at the value</param>
		/// <param name="stateOptions">The options to allow you to configure the options</param>
		/// <returns>The value holder</returns>
		public static RecoilState<T> UseRecoilState<T>(this FrameworkElement element, Selector<T> selector, RecoilStateOptions stateOptions = RecoilStateOptions.Default)
		{
			return UseRecoilStateInternal<T>(element, selector, stateOptions);
		}


		/// <summary>
		/// Once the control has been initialized thie will create a new instance the ViewModel of type T and assing it to the framework elements view model
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="element"></param>
		public static void UseRecoilViewModel<T>(this FrameworkElement element)
		{
			Type type = typeof(T);
			ConstructorInfo? constructorInfo = type.GetConstructors()
				.Where(t =>
				{
					ParameterInfo[]? parameters = t.GetParameters();
					return parameters.Length == 1 && parameters[0].ParameterType == typeof(IRecoilStore);
				})
				.FirstOrDefault();

			if (constructorInfo == null)
			{
				throw ErrorFactory.NoMatchingConstructorForStore(typeof(T));
			}

			UseRecoilViewModel<T>(element, s => (T)constructorInfo.Invoke(new object[] { s }));
		}


		/// <summary>
		/// Assings the <see cref="FrameworkElement.DataContext"/> value to a new instance of a <see cref="{T}"/> once
		/// the element has been loaded.
		/// </summary>
		/// <typeparam name="T">The type of the view model</typeparam>
		/// <param name="element">The element to assing</param>
		/// <param name="factory">The factory that can be used to create the view model</param>
		public static void UseRecoilViewModel<T>(this FrameworkElement element, Func<IRecoilStore?, T> factory)
		{
			element.InvokeWhenLoaded(() =>
			{
				IRecoilStore? store = XamlUtility.GetRecoilStore(element);
				element.DataContext = factory(store);
			});
		}



		private static RecoilState<T> UseRecoilStateInternal<T>(FrameworkElement element, RecoilValue<T> recoilValue, RecoilStateOptions stateOptions = RecoilStateOptions.Default)
		{
			RecoilState<T> state = new RecoilState<T>(recoilValue, null);

			if (element.IsInitialized && !stateOptions.HasFlag(RecoilStateOptions.AllowInitializedElements))
			{
				throw ErrorFactory.StateCreatedAfterComponentInitialized(element.GetType());
			}

			element.InvokeWhenLoaded(() =>
			{
				IRecoilStore store = element.GetRecoilStore();
				state.SetStore(store);
			});

			return state;
		}

	}
}
