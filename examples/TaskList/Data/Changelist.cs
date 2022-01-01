using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Data
{
	public class Changelist
	{
		/// <summary>
		/// Gets the changelist that we are dealing with
		/// </summary>
		public int ChangelistNumber { get; init; }

		/// <summary>
		/// Gets the perforce server the changelist belongs in
		/// </summary>
		public string? PerforcePort { get; init; }

		/// <summary>
		/// Gets the workspace the changelist is in
		/// </summary>
		public string? PerforceWorkspace { get; init; }
	}
}
