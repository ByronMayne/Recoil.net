using RecoilNet;
using System.Windows.Controls;
using TaskList.Data;
using TaskList.State;
using TaskList.ViewModel;

namespace TaskList.Components
{
	/// <summary>
	/// Interaction logic for TaskEditor.xaml
	/// </summary>
	public partial class TaskEditor : UserControl
	{
		public RecoilState<TaskData> SelectedTask { get; }

		public TaskEditor()
		{
			SelectedTask = this.UseRecoilState(TasksState.SelectedTask);
			SelectedTask.ValueChanged += OnSelectedTaskChanged;

			InitializeComponent();
		}

		private void OnSelectedTaskChanged(object? sender, TaskData? e)
		{
			DataContext = e == null
				? null
				: new TaskEditorViewModel(e);
		}

		private void ApplyChanges(object sender, System.Windows.RoutedEventArgs e)
		{
			SelectedTask.Value = ((TaskEditorViewModel)DataContext).ToTaskData();
		}
	}
}
