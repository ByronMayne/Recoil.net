using RecoilNet;
using RecoilNet.Effects;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RecoilNet.Values
{
    public class ValueProviderTests : RecoilTestBase
    {
        [Fact]
        public async Task Create_WithConstant_ReturnsConstantValue()
        {
            ValueProvider<int> provider = ValueProvider.Create(42);
            Assert.Equal(42, await provider.GetValueAsync(null, null));
        }

        [Fact]
        public async Task Create_WithFactory_ReturnsFactoryValue()
        {
            ValueProvider<string> provider = ValueProvider.Create(() => "factory");
            Assert.Equal("factory", await provider.GetValueAsync(null, null));
        }

        [Fact]
        public async Task Create_WithAsyncValue_ReturnsAsyncValue()
        {
            ValueProvider<int> provider = ValueProvider.Create(Task.FromResult(99));
            int result = await provider.GetValueAsync(null, null);
            Assert.Equal(99, result);
        }

        [Fact]
        public async Task Create_WithAsyncFactory_ReturnsAsyncFactoryValue()
        {
            ValueProvider<string> provider = ValueProvider.Create(() => Task.FromResult("asyncFactory"));
            var result = await provider.GetValueAsync(null, null);
            Assert.Equal("asyncFactory", result);
        }

        [Fact]
        public async Task Create_WithRecoilValue_ReturnsRecoilValue()
        {
            string expectedValue = "storeValue";
            Key key = Key.From("key");
            Atom<string> atom = CreateAtom("key", ValueProvider.Create(expectedValue));

            ValueProvider<string?> provider = ValueProvider.Create((s) => Recoil.GetValueAsync(s, atom));
            Assert.Equal(expectedValue, await provider.GetValueAsync(null, null));
        }
    }
}
