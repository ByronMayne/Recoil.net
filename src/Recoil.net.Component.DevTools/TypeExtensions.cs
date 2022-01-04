using Recoil.Components.DevTools;
using RecoilNet;

namespace RecoilNet
{
	/// <summary>
	/// Contains extension methods for adding in the dev tools
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Adds the development tools component to the generated store. This component allows the running application to be
		/// inspected by the Recoil.net.DevTools application.
		/// </summary>
		public static StoreConfigurationBuilder AddDevelopmentTools(this StoreConfigurationBuilder builder)
		{
			builder.AddComponent<DevToolsComponent>();
			return builder;
		}
	}
}