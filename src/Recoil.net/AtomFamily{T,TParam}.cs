using System;
using System.Collections.Generic;
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
    public delegate Atom<T> AtomFamily<T, TParam>(TParam parameter);


    internal class AtomFamilyNode<T, TParam> : Primitive<T> where TParam : notnull
    {

        public AtomFamilyNode(Key key) : base(key, false)
        { }

        public Atom<T> Get(TParam param)
        {
            
        }

        internal override string RenderDebug()
            => $"AtomFamily<{typeof(T).Name}, {typeof(TParam).Name}>: {Key}";
    }

}
