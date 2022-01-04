using RecoilNet.Components;
using RecoilNet.State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecoilNet
{
	/// <summary>
	/// Implemention used to build the configuration used by the store 
	/// </summary>
	public class StoreConfigurationBuilder
	{
		private readonly List<IStoreComponent> m_components;

		/// <summary>
		/// Gets the default settings to use for the configuration 
		/// </summary>
		public static StoreConfigurationBuilder Default 
			=> new StoreConfigurationBuilder();

		private StoreConfigurationBuilder()
		{
			m_components = new List<IStoreComponent>();
		}

		/// <summary>
		/// Adds a new component to the store that will be created when invoking build.
		/// </summary>
		public StoreConfigurationBuilder AddComponent<T>() where T : IStoreComponent, new()
			=> AddComponent<T>(new T());

		/// <summary>
		/// Adds a new component to the store that will be created when invoking build.
		/// </summary>
		public StoreConfigurationBuilder AddComponent<T>(T instance) where T : IStoreComponent
		{
			ArgumentNullException.ThrowIfNull(instance);
			m_components.Add(instance);
			return this;
		}

		/// <summary>
		/// Takes all the custom configurations and builds a new store instance 
		/// </summary>
		/// <returns></returns>
		public IRecoilStore Build()
		{
			return new RecoilStore(m_components);
		}
	}
}
