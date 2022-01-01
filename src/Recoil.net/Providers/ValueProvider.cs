using RecoilNet.Interfaces;
using RecoilNet.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecoilNet.Providers
{
	public class ValueProvider<T>  : IValueProvider
	{
		private readonly IRecoilStore? m_store;
		private readonly RecoilValue<T> m_recoilValue;

		public ValueProvider(IRecoilStore? store, RecoilValue<T> selector)
		{
			m_recoilValue = selector;
			m_store = store;
		}

		public async Task<TValue?> GetAsync<TValue>(Atom<TValue> atom)
		{
			ArgumentNullException.ThrowIfNull(atom);

			atom.AddDependent(m_recoilValue);
			return await atom.GetValueAsync(m_store);
		}

		public async Task<TValue?> GetAsync<TValue>(Selector<TValue> selector)
		{
			ArgumentNullException.ThrowIfNull(selector);
			selector.AddDependent(m_recoilValue);
			return await selector.GetValueAsync(m_store);
		}
	}
}
