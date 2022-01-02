using System.Collections.Generic;

namespace TaskList.Data
{
	public class TaskData
	{
		/// <summary>
		/// Gets the title of the task
		/// </summary>
		public string? Title { get; init; }

		/// <summary>
		/// Gets the description of the tats 
		/// </summary>
		public string? Description { get; init; }

		/// <summary>
		/// Gets the unique list of tags
		/// </summary>
		public IReadOnlyList<string>? Tags { get; init; }

		/// <summary>
		/// Gets if the task has been completed 
		/// </summary>
		public bool IsComplete { get; init; }
	}
}
