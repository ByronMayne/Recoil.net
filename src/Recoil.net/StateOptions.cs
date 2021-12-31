using System.ComponentModel;
using System.Windows;

namespace RecoilNet
{
	[Flags]
	public enum RecoilStateOptions
	{
		Default = 0,

		/// <summary>
		/// By default Recoil will now allow you to create new instance of <see cref="RecoilState{T}"/> on
		/// a <see cref="FrameworkElement"/> that already has it's InitializeComponent function called. This is because
		/// when using a property of type RecoilState you don't have to implement <see cref="INotifyPropertyChanged"/>. When the
		/// component is initialized it will get a null value and then when the state is assigned it will not update. With this option 
		/// you can create state after but you must implement the notifcation that the value changed.
		/// </summary>
		AllowInitializedElements = 1 << 1,

	}
}
