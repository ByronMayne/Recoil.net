using RecoilNet;
using RecoilNet.State;
using System.Dynamic;
using System.Windows;

namespace TaskList
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public IRecoilStore RecoilStore
		{
			get => (IRecoilStore)Resources[nameof(RecoilStore)];
			set => Resources[nameof(RecoilStore)] = value;
		}

		public App()
		{
			RecoilStore = StoreConfigurationBuilder
				.Default
				.AddDevelopmentTools()
				.Build();
		}
	}
}