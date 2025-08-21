using RecoilNet.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilNet.Values
{
    public class PrimitiveEvaluator<T>
    {
        private readonly Primitive<T> m_primitive;
        private readonly IRecoilStore? m_recoilStore;
        private readonly IList<Primitive> m_dependents;

        public PrimitiveEvaluator(IRecoilStore? recoilStore, Primitive<T> primitive)
        {
            Gaurd.NotNull(primitive);

            m_recoilStore = recoilStore;
            m_primitive = primitive;
            m_dependents = new List<Primitive>();
        }

        public Task<TValue?> GetAsync<TValue>(Atom<TValue> atom)
        {
            Gaurd.NotNull(atom);
            m_dependents.Add(atom);
            atom.AddDependent(m_primitive);
            return atom.GetValueAsync(m_recoilStore);
        }

        public Task<TValue?> GetAsync<TValue>(Selector<TValue> selector)
        {
            Gaurd.NotNull(selector);
            m_dependents.Add(selector);
            selector.AddDependent(m_primitive);
            return selector.GetValueAsync(m_recoilStore);
        }

        public Task<TValue?> GetAsync<TValue, TParam>(AtomFamily<TValue, TParam> atomFamily, TParam parameter) where TParam : notnull
        {
            Gaurd.NotNull(atomFamily);
            Gaurd.NotNull(parameter);
            Atom<TValue> atom = atomFamily(parameter);
            return GetAsync(atom);
        }

        //public Task<TValue?> GetAsync<TValue, TParam>(SelectorFamily<TValue, TParam> selectorFamily, TParam parameter) where TParam : notnull
        //{
        //    Gaurd.NotNull(selectorFamily);
        //    Gaurd.NotNull(parameter);
        //    Selector<TValue> selector = selectorFamily(parameter);
        //    return GetAsync(selector);
        //}

        public void Dispose()
        {
            foreach (Primitive dependent in m_dependents)
            {
              
            }
        }
    }
}
