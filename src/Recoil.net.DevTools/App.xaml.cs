using NodeNetwork;
using ReactiveUI;
using RecoilNet.State;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Recoil.net.DevTools
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{

		private readonly ProcessService m_processService;

		public IRecoilStore RecoilStore { get; }

		public App() : base()

		{
			RecoilStore = new RecoilStore();

			Resources[nameof(RecoilStore)] = RecoilStore;

			m_processService = new ProcessService(RecoilStore);
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			
			m_processService.Start();
		}
	}
}
