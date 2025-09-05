using RecoilNet.Diagnostics;
using RecoilNet.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RecoilNet
{
    public class RecoilStoreTests : RecoilTestBase
    {
        public RecoilStore Store { get; }

        public RecoilStoreTests(RecoilStore recoilStore)
        {
            Store = recoilStore;
        }

        [Fact]
        public async Task Get_Atom_Value_Returns_Default_If_Store_Has_No_Value()
        {
            int expectedValue = 1482;
            Atom<int> atom = CreateAtom<int>("IntValue", ValueProvider.Create(expectedValue));
            int actualValue = await Recoil.GetValueAsync(Store, atom, TestCancellation);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async Task Get_Atom_Value_Returns_Stored_Value_If_Store_Has_Value()
        {
            int expectedValue = 1482;
            Atom<int> atom = CreateAtom("IntValue", ValueProvider.Create(0));
            Recoil.SetValue(Store, atom, expectedValue);
            int actualValue = await Recoil.GetValueAsync(Store, atom, TestCancellation);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async Task Selectors_Wait_For_Dependency_Changes()
        {
            TaskCompletionSource source1 = new TaskCompletionSource();
            Selector<int> selector1 = Create<int>("Select1", async (get) =>
            {
                await source1.Task;
                return 123;
            });

            Selector<int> selector2 = Create<int>("Select2", async (get) =>
            {
                int selector1Value = await get.GetAsync(selector1);
                return selector1Value;
            });


            Task<int> select1Result = Recoil.GetValueAsync(Store, selector1, TestCancellation);
            Task<int> select2Result = Recoil.GetValueAsync(Store, selector2, TestCancellation);
            await Task.Delay(100, TestCancellation); // just to ensure the tasks have started

            Assert.False(select1Result.IsCompleted);
            Assert.False(select2Result.IsCompleted);

            source1.SetResult();
            await Task.Delay(100, TestCancellation);

            Assert.True(select1Result.IsCompleted);
            Assert.True(select2Result.IsCompleted);

            Assert.Equal(await select1Result, await select2Result);
        }

        private Selector<T> Create<T>(string name, Selector<T>.ValueGetter getter,

            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            CallerInfo creatorInfo = new CallerInfo(callerFilePath, callerMemberName, callerLineNumber);
            return new Selector<T>(key: Key.From(name), creatorInfo, getter);
        }
    }

}
