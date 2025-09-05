using RecoilNet.Utility;
using RecoilNet.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilNet
{
    public static class Recoil
    {
        public static void SetValue<T>(IRecoilStore? store, Primitive<T> primitive, T? value)
        {
            if(store is not null)
            {
                store.Set(primitive, value);
            }
        }

        public static async Task<T?> GetValueAsync<T>(IRecoilStore? store, Primitive<T> primitive, CancellationToken cancellationToken = default)
        {
            if (store is not null)
            {
                TryGetResult<object?> result = await store.TryGetAsync(primitive, cancellationToken);
                if (result)
                {
                    return (T?)result.Value;
                }
            }

            switch (primitive)
            {
                case Atom<T> atom:
                    return await atom.DefaultValue.GetValueAsync(store, null);
                case Selector<T> selector:
                    PrimitiveEvaluator<T> evaluator = new PrimitiveEvaluator<T>(store, selector);
                    return await selector.Getter(evaluator);
            }

            return default;
        }
    }
}
