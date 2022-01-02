using RecoilNet;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using TaskList.Commands;
using TaskList.Data;
using TaskList.State;

namespace TaskList.Components
{
	/// <summary>
	/// Interaction logic for TaskControls.xaml
	/// </summary>
	public partial class TaskControls : UserControl
	{

		/// <summary>
		/// The task that is invoked whenever the user requests to add a command 
		/// </summary>
		public ICommand AddTaskCommand { get; }

		/// <summary>
		/// The command that is invoked whenver the user wants to remove a command 
		/// </summary>
		public ICommand RemoveTaskCommand { get; }

		/// <summary>
		/// Gets the first name of the user 
		/// </summary>
		public RecoilState<IReadOnlyList<TaskData>> Tasks { get; }

		/// <summary>
		/// Gets the currently selected task 
		/// </summary>
		public RecoilState<TaskData> SelectedTask { get; }

		public TaskControls()
		{
			DataContext = this;

			// State 
			Tasks = this.UseRecoilState(TasksState.Tasks);
			SelectedTask = this.UseRecoilState(TasksState.SelectedTask);

			// Commands 
			AddTaskCommand = new RelayCommand(AddNewTask);
			RemoveTaskCommand = new RelayCommand(RemoveTask);

			InitializeComponent();
		}

		/// <summary>
		/// Removes the selected item if we have one
		/// </summary>
		private void RemoveTask(object? obj)
		{
			Tasks.Remove(SelectedTask.Value);
		}

		/// <summary>
		/// Adds a new item if we have one 
		/// </summary>
		private void AddNewTask(object? obj)
		{
			Tasks.Add(new TaskData()
			{
				Description = "This is a description",
				IsComplete = false,
				Title = $"{Tasks?.Value?.Count + 1}: Task"
			});
		}
	}
}
