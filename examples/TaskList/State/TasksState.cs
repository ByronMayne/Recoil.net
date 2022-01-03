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
		public static readonly Atom<int> SelectedTaskIndex;
		public static readonly Atom<IReadOnlyList<TaskData>> Tasks;
		public static readonly Selector<TaskData> SelectedTask;


		static TasksState()
		{
			SelectedTaskIndex = new Atom<int>("TaskState.SelectedTaskIndex");
			Tasks = new Atom<IReadOnlyList<TaskData>>("TaskState.Tasks", Array.Empty<TaskData>());


			SelectedTask = new Selector<TaskData>("TaskState.SelectedTask", GetSelectedTask, SetSelectedTask);
		}


		/// <summary>
		/// Using the list of tasks this returns back the instance at that given index
		/// </summary>
		private static async Task<TaskData?> GetSelectedTask(IValueProvider provider)
		{
			IReadOnlyList<TaskData>? tasks = await provider.GetAsync(Tasks);
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
		private static async Task SetSelectedTask(IRecoilStore store, TaskData selectedValue)
		{
			int index = await SelectedTaskIndex.GetValueAsync(store);
			IReadOnlyList<TaskData>? tasks = await Tasks.GetValueAsync(store);

			if (tasks == null) return;

			List<TaskData> result = tasks.ToList();
			result[index] = selectedValue;
			Tasks.SetValue(store, result);
		}
	}
}
