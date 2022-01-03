using NUnit.Framework;
using RecoilNet;
using RecoilNet.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Recoil.net.Tests
{
	public class SelectorTests
	{

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		[Test]
		public void Create_WithNullKey_ThrowsArgumentNullException()
			=> Assert.Throws<ArgumentNullException>(() => new Selector<string>(null, DefaultBuilder<string>),
				   "We should have received an exception for the null key");

		[Test]
		public void Create_WithNulBuilder_ThrowsArgumentNullException()

			=> Assert.Throws<ArgumentNullException>((TestDelegate)(() => new Selector<string>("KEY", getter:(Selector<string>.ValueGetter?)null)),
				   "We should have received an exception for the null key");
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

		[Test]
		public void Value_MatchesExpected()
		{
			const string FIRST_NAME = "John";
			const string LAST_NAME = "Smith";
			Atom<string> FirstNameAtom = new Atom<string>("FirstName", FIRST_NAME);
			Atom<string> LastNameAtom = new Atom<string>("LastName", LAST_NAME);
			Selector<string> FullNameSelector = new Selector<string>("FullName", async get => $"{await get.GetAsync(FirstNameAtom)} {await get.GetAsync(LastNameAtom)}");

			Assert.AreEqual($"{FIRST_NAME} {LAST_NAME}", FullNameSelector.GetValueAsync(null));
		}

		[Test]
		public async void DependenciesAdded_When_FetchingValue()
		{
			Atom<string> atomDep = new Atom<string>("");
			Selector<string> selectorDep = new Selector<string>("", DefaultBuilder<string>);

			Selector<string> selector = new Selector<string>("Selector", async (builder) =>
			{
				var _1 = await builder.GetAsync(atomDep);
				var _2 = await builder.GetAsync(selectorDep);
				return "";
			});

			await selector.GetValueAsync(null);

			// We should not
			Assert.AreEqual(0, selector.Dependents.Count, "The selector should not have any dependents");
			Assert.AreEqual(1, atomDep.Dependents.Count, "We should have 1 dependent");
			Assert.AreEqual(1, selectorDep.Dependents.Count, "We should have 1 dependent");
		}

		[Test]
		public void OtherTest()
		{
			RecoilState<string> stateValue = new RecoilState<string>(new Atom<string>("Atom", ""), null);
		}

		private static Task<T?> DefaultBuilder<T>(IValueProvider _)
			=> Task.FromResult(default(T));
	}
}