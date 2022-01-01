using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecoilNet.Interfaces
{
    /// <summary>
    /// Defines an object which can evaluate the value of another atom
    /// </summary>
    public interface IValueProvider
    {
        /// <summary>
        /// Gets the value of an atom 
        /// </summary>
        /// <typeparam name="TValue">The type of the atom</typeparam>
        /// <param name="atom">The atom to get the value of</param>
        /// <returns>The value of the atom</returns>
        public Task<T?> GetAsync<T>(Atom<T> atom);

        /// <summary>
        /// Gets the value from a selector
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="selector">The selector to use</param>
        /// <returns>The value of the selector</returns>
        public Task<T?> GetAsync<T>(Selector<T> selector);
    }
}
