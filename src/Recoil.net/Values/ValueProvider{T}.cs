namespace RecoilNet.Values
{
    /// <summary>
    /// Provides a mechanism to evaluate and supply values of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of value provided.</typeparam>
    public class ValueProvider<T>
    {
        /// <summary>
        /// Creates a value provider that always returns the default value of type <typeparamref name="T"/>.
        /// </summary>
        public static ValueProvider<T> Default { get; }

        /// <summary>
        /// The delegate used to evaluate the value.
        /// </summary>
        private readonly Delegate m_evaluation;

        /// <summary>
        /// The fixed parameter for evaluation, if any.
        /// </summary>
        private readonly object? m_parameter;

        /// <summary>
        /// The type of the parameter, if any.
        /// </summary>
        private readonly Type? m_parameterType;

        /// <summary>
        /// The method signature used by the value provider.
        /// </summary>
        private readonly ValueProviderMethodSignature m_signature;

        /// <summary>
        /// The type of the value provider.
        /// </summary>
        private readonly ValueProviderType m_type;

        /// <summary>
        /// Gets the underlying implementation type of the value provider.
        /// </summary>
        public ValueProviderType Type => m_type;

        /// <summary>
        /// Gets the underlying delegate method signature that is used to evaluate the value.
        /// </summary>
        public ValueProviderMethodSignature Signature => m_signature;

        static ValueProvider()
        {
            Default = ValueProvider.Create(default(T));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueProvider{T}"/> class.
        /// </summary>
        /// <param name="evaluation">The delegate used to evaluate the value.</param>
        /// <param name="type">The type of the value provider.</param>
        /// <param name="signature">The method signature used by the value provider.</param>
        internal ValueProvider(
            Delegate evaluation,
            ValueProviderType type,
            ValueProviderMethodSignature signature)
        {
            Guard.NotNull(evaluation);
            Guard.IsDefined(signature);
            Guard.IsDefined(type);

            m_type = type;
            m_signature = signature;
            m_parameter = null;
            m_parameterType = null;
            m_evaluation = evaluation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueProvider{T}"/> class with a fixed parameter.
        /// </summary>
        /// <param name="evaluation">The delegate used to evaluate the value.</param>
        /// <param name="type">The type of the value provider.</param>
        /// <param name="signature">The method signature used by the value provider.</param>
        /// <param name="fixedParameter">The fixed parameter for evaluation.</param>
        /// <param name="parameterType">The type of the parameter.</param>
        internal ValueProvider(
            Delegate evaluation,
            ValueProviderType type,
            ValueProviderMethodSignature signature,
            object? fixedParameter,
            Type parameterType)
        {
            Guard.NotNull(evaluation);
            Guard.IsDefined(signature);

            m_type = type;
            m_signature = signature;
            m_evaluation = evaluation;
            m_parameter = fixedParameter;
            m_parameterType = parameterType;
        }

        /// <summary>
        /// Converts the value to a fixed parameter for the value provider.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public ValueProvider<T> WithParameter(object? parameter)
        {
            if (m_parameterType is null)
            {
                throw new InvalidOperationException("This ValueProvider does not support parameters.");
            }
            if (parameter is not null && !m_parameterType.IsAssignableFrom(parameter.GetType()))
            {
                throw new ArgumentException($"The parameter must be of type {m_parameterType}.", nameof(parameter));
            }
            return new ValueProvider<T>(
                m_evaluation,
                m_type,
                m_signature,
                parameter,
                m_parameterType);
        }

        public Task<T?> GetValueAsync(IRecoilStore? recoilStore, object? parameter)
        {
            return InternalGetValueAsync(recoilStore, parameter);
        }

        private Task<T?> InternalGetValueAsync(IRecoilStore? recoilStore, object? parameter)
        {
            object?[] arguments = m_signature switch
            {
                ValueProviderMethodSignature.Parameter => [parameter],
                ValueProviderMethodSignature.Store => [recoilStore],
                ValueProviderMethodSignature.Store | ValueProviderMethodSignature.Parameter => [recoilStore, parameter ],
                _ => Array.Empty<object?>(),
            };

            object? result = m_evaluation.DynamicInvoke(arguments);
            return result is Task<T?> asTask ? asTask : Task.FromResult((T?)result);
        }
    }
}
