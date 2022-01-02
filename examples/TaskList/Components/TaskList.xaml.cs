using RecoilNet;
using System.Collections.Generic;
using System.Windows.Controls;
using TaskList.Data;
using TaskList.State;

namespace TaskList.Components
{
	/// <summary>
	/// Interaction logic for TaskList.xaml
	/// </summary>
	public partial class TaskList : UserControl
	{
		/// <summary>
		/// Gets the first name of the user 
		/// </summary>
		public RecoilState<IReadOnlyList<TaskData>> Tasks { get; }

		/// <summary>
		/// Gets the currently selected index
		/// </summary>
		public RecoilState<int> SelectedIndex { get; }

		public TaskList()
		{
			DataContext = this;

			Tasks = this.UseRecoilState(TasksState.Tasks);
			SelectedIndex = this.UseRecoilState(TasksState.SelectedTaskIndex);

			InitializeComponent();
		}
	}
}
