using System.ComponentModel;
using System.Windows;

namespace RecoilNet.Utility
{
	/// <summary>
	/// Contains helper methods for formatting errors that happen at runtime 
	/// </summary>
	public static class ErrorFactory
	{
		public static RecoilException StateCreatedAfterComponentInitialized(Type elementType)
		{
			return new RecoilException($@"
The Framework element '{elementType}' has already been initialized. You must create Recoil State objects before 'InitializeComponent' otherwise bindings will fail. You can 
also avoid this exception by setting the argument {nameof(RecoilStateOptions.AllowInitializedElements)} when creating the state but you *must* implement {nameof(INotifyPropertyChanged)} so the
value gets picked up.

Example of the proper component initialization.
--------------------------
public class MyClass : FrameworkElement 
{{
    public RecoilState<string> StringValue {{ get; }}

    public MyClass()
    {{
        // Initialize State Objects 
        StringValue = this.UseRecoilState(StringValueAtom);
        
        // Invoke Component initialization
        InitializeComponent();
    }}
}}
--------------------------");
		}

		/// <summary>
		/// Exception thrown while trying to set the value of an object that is not mutable
		/// </summary>
		public static RecoilException AssigningValueToNonMutableType<T>(Primitive<T> objectType)
		{
			string error = $"You are unable to asssing a value to the object of type '{objectType.GetType().Name}' of the key '{objectType.Key}' because " +
				$"it's marked at not mutable.";
			return new RecoilException(error);
		}

		public static RecoilException NoMatchingConstructorForStore(Type type)
		{
			string error = $"The type {type.FullName} does not contain a constructor that takes a single argument of type {typeof(IRecoilStore).FullName}. It must have one";
			throw new RecoilException(error);
		}
	}
}
