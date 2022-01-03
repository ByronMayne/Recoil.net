# Selectors 

Selectors represent derived state. Similar to [Atoms]('./../Atoms.md) they require a unique string key which is used to store the value. This key must be unique across the whole application otherwise an exception will be thrown.

Selectors should be static and accessible to the whole application. 

```cs
public static class AppState
{
	public static readonly Atom<int> SelectedIndex;
	public static readonly Atom<IReadOnlyList<Fruit>> Fruits;
	public static readonly Selector<Fruit> SelectedFruit;

	static AppState
	{
		SelectedIndex = new Atom<int>("Fruit.SelectedIndex");
		Fruits = new Atom<IReadOnlyList<Fruit>>("Fruits.FruitList");
		SelectedFruit = new Selector<Fruit>("Fruits.Selected", GetSelectedFruit);
	}

	// Pure function for selecting our fruit
	private static async Task<Fruit> GetSelectedFruit(IValueProvider provider)
	{
		IReadOnlyList<Fruit>? fruits = await provider.GetAsync(Fruits);
		int index = await provider.GetAsync(SelectedIndex);

		if(index < 0 || fruits == null || index >= fruits.Count)
		{
			// We have an invalid index or no fruits in our list
			return null;
		}
		return fruits[index];
	}
}
```

Unlike Atoms Selectors don't have a default value but instead have a delegate to a method. This method is used to compute derived state from other selectors or atoms. In this case we get the list of `Fruit` from one Atom and the selected `Index` of another. We then return that `Fruit` at that index. 

Whenever our the `Fruits` or `SelectedIndex` atoms value changes our `SelectedFruit` selector will reevaluate. Outside of that the value will always be cached.

One advantage here is that things that care just about the selected fruit and not about the other don't have to worry about calculating it themselves. 


## Setters 

By default all selectors are read only and if you try to set them from a `RecoilState.Value` you will get an exception, however they do support being set. Lets take our example from above 


```cs
public static class AppState
{
	public static readonly Atom<int> SelectedIndex;
	public static readonly Atom<IReadOnlyList<Fruit>> Fruits;
	public static readonly Selector<Fruit> SelectedFruit;

	static AppState
	{
		SelectedIndex = new Atom<int>("Fruit.SelectedIndex");
		Fruits = new Atom<IReadOnlyList<Fruit>>("Fruits.FruitList");

		SelectedFruit = new Selector<Fruit>("Fruits.Selected", GetSelectedFruit, SetSelectedFruit);
	}

	private static Task SetSelectedFruit(IRecoilStore store, Fruit fruit)
	{
		int index = await SelectedIndex.GetValueAsync(store);
		IReadOnlyList<Fruit>? fruits = await Fruits.GetValueAsync(store);

		if(fruits == null) return;

		List<Fruit> result = fruits.ToList();
		result[index] = fruit;
		Fruits.SetValue(store, result);
	}

	// private static async Task<Fruit> GetSelectedFruit(IValueProvider provider)
	// ... trimmed for brevity
}
```

We have added another argument to our fruit selector `SetSelectedFruit`. This function allows us to replace the value of the selected fruit. Previously if anyone would want to replace the value they would need to write the logic themselves that we have in the set function.

now the following can be done.

```cs
public class FruitDisplay : UserControl
{
	public RecoilState<Fruit> SelectedFruit { get; }

	public FruitDisplay()
	{
		SelectedFruit = this.UseRecoilState(AppState.SelectedFruit);
		InitializeComponent();
	}

	public void SetFruitPrice(float price)
	{
		Fruit fruit = SelectedFruit.Value;
		// Without defining the setter on the Selector `.Value` would have thrown an exception
		SelectedFruit.Value = fruit with { Price = sprite };
	}
}
```

Now we can update the price of the selected fruit without having to care about the list or the selected index.