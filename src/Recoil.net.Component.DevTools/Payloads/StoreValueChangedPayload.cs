using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recoil.Components.DevTools.Payloads
{
	internal record StoreValueChangedPayload
	{
		/// <summary>
		/// Gets the unique id of the store that raised the event
		/// </summary>
		public int StoreId { get; init; }

		/// <summary>
		/// Gets the key that the value belongs too
		/// </summary>
		public string? Key { get; init; }

		/// <summary>
		/// Gets the list of the keys that have been changed because they depended
		/// on the <see cref="Key"/>
		/// </summary>
		public IReadOnlyList<string>? DependentKeys { get; init; }

		/// <summary>
		/// Gets the type of the value that changed 
		/// </summary>
		public string? ValueTypeName { get; init; }

		/// <summary>
		/// Gets or sets if the change had a value or if it was null
		/// </summary>
		public bool HasValue { get; init; }

		/// <summary>
		/// Gets the json string of the object that changed 
		/// </summary>
		public string? JsonValue { get; init; }
	}
}
