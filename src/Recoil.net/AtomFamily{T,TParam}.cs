using RecoilNet.Diagnostics;
using RecoilNet.Effects;
using RecoilNet.Values;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RecoilNet
{

    /// <summary>
    /// Represents a factory delegate for creating <see cref="Atom{T}"/> instances based on a parameter.
    /// This is useful for generating parameterized atoms in a state management context.
    /// </summary>
    /// <typeparam name="T">The type of the value held by the atom.</typeparam>
    /// <typeparam name="TParam">The type of the parameter used to create the atom.</typeparam>
    /// <param name="parameter">The parameter used to create the atom instance.</param>
    /// <returns>An <see cref="Atom{T}"/> instance associated with the given parameter.</returns>
    public delegate Atom<T> AtomFamily<T, TParam>(TParam parameter,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0);


    internal class AtomFamilyNode<T, TParam> : Primitive<T> where TParam : notnull
    {
        private readonly Dictionary<TParam, Atom<T>> m_instances;
        private readonly ValueProvider<T> m_defaultValueProvider;
        private readonly IPrimitiveEffect<T>[] m_effects;


        public AtomFamilyNode(
            Key key, 
            CallerInfo creatorInfo, 
            ValueProvider<T> defaultValueProvider,
            IEnumerable<IPrimitiveEffect<T>> effects) : base(key, creatorInfo, false)
        {
            m_instances = new Dictionary<TParam, Atom<T>>();
            m_effects = effects.ToArray();
            m_defaultValueProvider = defaultValueProvider;

        }

        public Atom<T> Get(TParam param,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            if (!m_instances.TryGetValue(param, out Atom<T> instance))
            {
                Key childKey = Key.CreateChildKey(Key, param);
                CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
                ValueProvider<T> atomProvider = m_defaultValueProvider.WithParameter(param);
                instance = new Atom<T>(childKey, creatorInfo, atomProvider, m_effects);
                m_instances[param] = instance;
            }

            return instance;
        }

        internal override string RenderDebug()
            => $"AtomFamily<{typeof(T).Name}, {typeof(TParam).Name}>: {Key}";
    }

}
