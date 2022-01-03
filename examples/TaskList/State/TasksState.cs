using RecoilNet;
using RecoilNet.Interfaces;
using RecoilNet.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskList.Data;

namespace TaskList.State
{
	public static class TasksState
	{
		public static readonly Atom<string> TaskFilterString;
		public static readonly Atom<bool> FilterIsCaseSensetive;

		public static readonly Atom<int> SelectedTaskIndex;
		public static readonly Atom<IReadOnlyList<TaskData>> Tasks;
		public static readonly Selector<TaskData> SelectedTask;
		public static readonly Selector<IReadOnlyList<TaskData>> FilteredTasks;


		static TasksState()
		{
			TaskFilterString = new Atom<string>("TaskState.Filter", "");
			SelectedTaskIndex = new Atom<int>("TaskState.SelectedTaskIndex");
			FilterIsCaseSensetive = new Atom<bool>("TaskState.Filter.CaseSensetive", false);
			Tasks = new Atom<IReadOnlyList<TaskData>>("TaskState.Tasks", Array.Empty<TaskData>());


			SelectedTask = new Selector<TaskData>("TaskState.SelectedTask", GetSelectedTask, SetSelectedTask);
			FilteredTasks = new Selector<IReadOnlyList<TaskData>>("TaskState.FilteredTasks", GetFiltredTasks);
		}

		private static async Task<IReadOnlyList<TaskData>?> GetFiltredTasks(IValueProvider provider)
		{
			string? filter = await provider.GetAsync(TaskFilterString);
			bool caseSensetive = await provider.GetAsync(FilterIsCaseSensetive);
			IReadOnlyList<TaskData>? tasks = await provider.GetAsync(Tasks);

			// no filter just return our other list 
			if (string.IsNullOrWhiteSpace(filter) || tasks == null) return tasks;

			StringComparison comparison = caseSensetive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

			List<TaskData?> filteredTasks = new List<TaskData?>();

			foreach(TaskData taskData in tasks)
			{
				string title = taskData.Title ?? "";
				string description = taskData.Description ?? "";
				
				if(title.Contains(filter, comparison) || description.Contains(filter, comparison))
				{
					filteredTasks.Add(taskData);
				}
			}

			return filteredTasks!;

		}


		/// <summary>
		/// Using the list of tasks this returns back the instance at that given index
		/// </summary>
		private static async Task<TaskData?> GetSelectedTask(IValueProvider provider)
		{
			IReadOnlyList<TaskData>? tasks = await provider.GetAsync(FilteredTasks);
			int taskIndex = await provider.GetAsync(SelectedTaskIndex);

			if (taskIndex < 0 || tasks == null || taskIndex >= tasks.Count)
			{
				return null;
			}

			return tasks[taskIndex];
		}

		/// <summary>
		/// Allows anyone to replace the value of the currently selected state
		/// </summary>
		private static async Task SetSelectedTask(IRecoilStore store, TaskData? selectedValue)
		{
			int index = await SelectedTaskIndex.GetValueAsync(store);
			IReadOnlyList<TaskData?>? tasks = await FilteredTasks.GetValueAsync(store);

			if (tasks == null) return;

			List<TaskData?> result = tasks.ToList();
			result[index] = selectedValue;
			await Tasks.SetValueAsync(store, result!);

			// Now we have to set back the index because WPF changes it to -1 when switching sources
			await SelectedTaskIndex.SetValueAsync(store, index);
		}
	}
}
