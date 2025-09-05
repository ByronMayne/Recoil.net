using RecoilNet.Diagnostics;
using RecoilNet.Utility;
using RecoilNet.Values;

namespace RecoilNet
{


    /// <summary>
    /// A selector represents a piece of derived state. You can think of 
    /// derived state as the output of passing state to a pure function 
    /// that modifies the given state in some way.
    /// </summary>
    public class Selector<T> : Primitive<T>
    {
        /// <summary>
        /// Delegate for asynchronously retrieving the value of the selector.
        /// </summary>
        /// <param name="getter">The evaluator used to access other atoms/selectors.</param>
        /// <returns>A task that resolves to the value of the selector.</returns>
        public delegate Task<T?> ValueGetter(PrimitiveEvaluator<T> getter);

        /// <summary>
        /// Delegate for asynchronously setting the value of the selector.
        /// </summary>
        /// <param name="provider">The recoil store provider.</param>
        /// <param name="Value">The value to set.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public delegate Task ValueSetter(IRecoilStore provider, T? Value);

        /// <summary>
        /// The method used to evaluate the value of the selector
        /// </summary>
        public readonly ValueGetter Getter;

        /// <summary>
        /// Gets the optional method used to set the value of the selector.
        /// </summary>
        public readonly ValueSetter? Setter;

        /// <summary>
        /// Initializes a new instance of a readonly selector using the specified getter method.
        /// </summary>
        /// <param name="key">A unique key for the selector.</param>
        /// <param name="creatorInfo">Information about the creator of this selector, used for debugging.</param>
        /// <param name="getter">The method to get the value of the selector.</param>
        public Selector(Key key, CallerInfo creatorInfo, ValueGetter getter)
            : base(key, creatorInfo, false)
        {
            Guard.NotNull(getter);
            Getter = getter;
        }

        /// <summary>
        /// Initializes a new instance of a mutable selector using the specified getter and setter methods.
        /// </summary>
        /// <param name="key">A unique key for the selector.</param>
        /// <param name="creatorInfo">Information about the creator of this selector, used for debugging.</param>
        /// <param name="getter">The method to get the value of the selector.</param>
        /// <param name="setter">The method to set the value of the selector.</param>
        public Selector(Key key, CallerInfo creatorInfo, ValueGetter getter, ValueSetter setter)
            : base(key, creatorInfo, true)
        {
            Guard.NotNull(getter);
            Guard.NotNull(setter);
            Getter = getter;
            Setter = setter;
        }

        /// <summary>
        /// Renders a debug string representation of the selector for use in tooltips and debugging.
        /// </summary>
        /// <returns>A string describing the selector and its key.</returns>
        internal override string RenderDebug()
            => $"Selector<{typeof(T).Name}>: {Key}";
    }
}
