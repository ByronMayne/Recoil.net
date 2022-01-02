using RecoilNet;
using System;
using System.Collections.Generic;
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
			SelectedTask = new Selector<TaskData>("TaskState.SelectedTask", async builder =>
			{
				IReadOnlyList<TaskData>? tasks = await builder.GetAsync(Tasks);
				int taskIndex = await builder.GetAsync(SelectedTaskIndex);

				if (taskIndex < 0 || tasks == null || taskIndex >= tasks.Count)
				{
					return null;
				}

				return tasks[taskIndex];
			});
		}
	}
}
