using RecoilNet.State;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
            Gaurd.NotNull(evaluation);
            Gaurd.IsDefined(signature);
            Gaurd.IsDefined(type);

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
            Gaurd.NotNull(evaluation);
            Gaurd.IsDefined(signature);

            m_type = type;
            m_signature = signature;
            m_evaluation = evaluation;
            m_parameter = fixedParameter;
            m_parameterType = parameterType;
        }
    }
}
