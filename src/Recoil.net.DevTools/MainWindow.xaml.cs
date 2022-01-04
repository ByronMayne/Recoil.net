using Recoil.net.DevTools.Data;
using Recoil.net.DevTools.State;
using RecoilNet;
using System.Collections.Generic;
using System.Windows;

namespace Recoil.net.DevTools
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public RecoilState<int> SelectedProcessIndex { get; }
		public RecoilState<ProcessTarget> SelectedProcess { get; }
		public RecoilState<IReadOnlyList<ProcessTarget>> Processes { get; }

		public MainWindow()
		{
			DataContext = this;

			SelectedProcessIndex = this.UseRecoilState(ProcessState.SelectedProcessIndex);
			SelectedProcess = this.UseRecoilState(ProcessState.SelectedProcess);
			Processes = this.UseRecoilState(ProcessState.Processes);

			InitializeComponent();

			//var network = new NetworkViewModel();
			//var node1 = new NodeViewModel();
			//node1.Name = "Node 1";
			//network.Nodes.Add(node1);

			//var node1Input = new NodeInputViewModel();
			//node1Input.Name = "Node 1 input";
			//node1.Inputs.Add(node1Input);

			//var node2 = new NodeViewModel();
			//node2.Name = "Node 2";
			//network.Nodes.Add(node2);


			//var node2Output = new NodeOutputViewModel();
			//node2Output.Name = "Node 2 output";
			//node2.Outputs.Add(node2Output);


			////Assign the viewmodel to the view.
			//networkView.ViewModel = network;
		}
	}
}
