using RecoilNet;
using System.Windows.Controls;
using TaskList.State;

namespace TaskList.Components
{
	/// <summary>
	/// Interaction logic for TaskFilterBox.xaml
	/// </summary>
	public partial class TaskFilterBox : UserControl
	{
		public RecoilState<string> Filter { get; }

		public TaskFilterBox()
		{
			DataContext = this;

			Filter = this.UseRecoilState(TasksState.TaskFilterString);

			InitializeComponent();
		}
	}
}
