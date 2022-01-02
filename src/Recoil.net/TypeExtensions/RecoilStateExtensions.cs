using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace RecoilNet
{
	/// <summary>
	/// Contains extension methods for working with Recoil state
	/// </summary>
	public static class RecoilStateExtensions
	{
		/// <summary>
		/// Copies the existing collection and adds the new element to it. The original value is
		/// not modified 
		/// </summary>
		/// <typeparam name="T">The type of the item</typeparam>
		/// <param name="state">The state the item belongs in</param>
		/// <param name="item">The new item to add</param>
		public static void Add<T>(this RecoilState<IReadOnlyList<T>> state, T? item)
		{
			T?[] result;

			if (state.Value == null)
			{
				result = new T?[] { item };
			}
			else
			{

				T?[] values = new T[state.Value.Count + 1];

				for (int i = 0; i < state.Value.Count; i++)
				{
					values[i] = state.Value[i];
				}
				values[^1] = item;
				result = values;
			}
			state.Value = result!;
		}


		/// <summary>
		/// Loops over the current values and removes all elements are are equal to the sent in item. If the collection
		/// is null or empty nothing will happen. This does not modify the underlaying value but instead creates a new one.
		/// </summary>
		/// <typeparam name="T">The type of the item</typeparam>
		/// <param name="state">The state that holds the value</param>
		/// <param name="item">The item to remove</param>
		/// <returns>True if an item was removed otherwise false</returns>
		public static bool Remove<T>(this RecoilState<IReadOnlyList<T>> state, T? item)
		{
			if (state.Value == null) return false;

			List<T> result = new List<T>();

			for(int i = 0; i < state.Value.Count; i++)
			{
				if(!EqualityComparer<T>.Default.Equals(state.Value[i], item))
				{
					result.Add(state.Value[i]);
				}
			}

			bool wasRemoved = result.Count != state.Value.Count;
			state.Value = result;
			return wasRemoved;
		}

		/// <summary>
		/// Removes the item at the selected index. If the index is out of range nothing happens. This does not modify the collection
		/// but instead creates a new copy and sets the underlaying value property 
		/// </summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <param name="state">The state that holds the value</param>
		/// <param name="index">The index to remove from</param>
		/// <returns>True if a value was remove otherwise false</returns>
		public static bool RemoveAtIndex<T>(this RecoilState<IReadOnlyList<T>> state, int index)
		{
			IReadOnlyList<T>? items = state.Value;

			// Don't remove if it's not there 
			if(items == null || index < 0 || index >= items.Count) return false;

			T[] result = new T[items.Count - 1];

			int dstIndex = 0;

			for(int srcIndex = 0; srcIndex < items.Count; srcIndex++)
			{
				if (srcIndex == index) continue;

				result[dstIndex]= items[srcIndex];
				dstIndex++;
			}

			state.Value = result;

			return true;
		}
	}
}
