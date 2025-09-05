using RecoilNet.Diagnostics;
using RecoilNet.Effects;
using RecoilNet.Values;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Xunit;

namespace RecoilNet
{
    public class RecoilTestBase
    {
        public CancellationToken TestCancellation => TestContext.Current.CancellationToken;



        protected Atom<T> CreateAtom<T>(string name,
            ValueProvider<T>? defaultValue = null,
            IPrimitiveEffect<T>[]? effects = null,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            Guard.NotNull(name);

            effects ??= Array.Empty<IPrimitiveEffect<T>>();
            defaultValue ??= ValueProvider<T>.Default;
            CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);

            Key key = Key.From(name);
            return new Atom<T>(key, creatorInfo, defaultValue, effects);
        }

        protected Selector<T> CreateSelector<T>(string name,
            Selector<T>.ValueGetter getter,
            ValueProvider<T>? defaultValue = null,
            IPrimitiveEffect<T>[]? effects = null,
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            Guard.NotNull(name);
            Guard.NotNull(getter); ;

            effects ??= Array.Empty<IPrimitiveEffect<T>>();
            defaultValue ??= ValueProvider<T>.Default;
            CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);

            Key key = Key.From(name);
            return new Selector<T>(key, creatorInfo, getter);
        }
    }
}
