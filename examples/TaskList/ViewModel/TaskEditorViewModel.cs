using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskList.Data;

namespace TaskList.ViewModel
{
	public class TaskEditorViewModel
	{
		/// <summary>
		/// Gets or sets the title of the task
		/// </summary>
		public string? Title { get; set; }

		/// <summary>
		/// Gets or sets the description of the taks
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// Gets or sets the labels of the task
		/// </summary>
		public List<string> Tags { get; set; }


		public TaskEditorViewModel(TaskData taskData)
		{
			Title = taskData.Title;
			Description = taskData.Description;
			Tags = new List<string>(taskData.Tags);
		}

		public TaskData ToTaskData()
		{
			return new TaskData()
			{
				Title = Title,
				Description = Description,
				Tags = Tags,
			};
		}
	}
}
