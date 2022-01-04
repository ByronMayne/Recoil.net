using Recoil.net.DevTools.Data;
using Recoil.net.DevTools.State;
using RecoilNet;
using System.Windows.Controls;

namespace Recoil.net.DevTools.Controls
{
	/// <summary>
	/// Interaction logic for ProcessInspectorControl.xaml
	/// </summary>
	public partial class ProcessInspectorControl : UserControl
	{
		public ProcessInspectorControl()
		{
			this.UseRecoilStateDataContext(ProcessState.ProcessInspector);

			InitializeComponent();
		}
	}
}
