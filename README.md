# Recoil.net

Recoil.net is the C# version of the Facebook's [Recoil.js](https://recoiljs.org/) library. It's designed to handle managing state in WPF applications. There are 3 main goals this library is intended to do.

#### Flexible shared state
 - The ability to have different things in syncs in sync in different parts of the application.

#### Derived data and queries
 - The ability to compute things based on changing state efficiently in a way that is very robust so that we can move quickly and not have bugs.

#### App-wide state observation 
 - For things like time travel debugging, undo support, persistence logging etc.
 - Ability to view what is going on across the whole application 


## Getting Started


### Creating the Root

The first thing that needs to be added to the root if your WPF application in the `<RecoilRoot>`. This is where all the data will be stored for your application. In most cases you will only need a single one but multiple is supported.


```xml
<Window x:Class="ExampleWindow"
        xmlns:recoil="clr-namespace:RecoilSharp;assembly=RecoilSharp"
        xmlns:recoilsharp="clr-namespace:RecoilSharp;assembly=RecoilSharp">
    <recoilsharp:RecoilRoot>
        <!-- Application code lives here -->
    </recoilsharp:RecoilRoot>
</Window>
```

Inside the `<RecoilRoot>` an instance of the `IRecoilStore` will be created. This is where all children will store their values. Whenever you create a new `RecoilState<T>` object it will walk the hierarchy and use the first root it finds. If none is found an exception will be thrown.

### Defining State 

With Recoil all state is defined using one of two types `Atom<T>` or `Selector<T>`. These Recoil value objects are keys to data that exists in the `IRecoilStore` and each state object *must* have a unique key. 

```csharp
public static class UserState
{
    public static readonly Atom<string> FirstName;
    public static readonly Atom<string> LastName;
    public static readonly Selector<string> Username { get; }

    static UserState
    {
        // Recoil.CreateAtom<T>(string: key, defaultValue: T | Atom<T> | Selector<T>)
        FirstName = Recoil.CreateAtom<string>("User.FirstName", "John");
        LastName = Recoil.CreateAtom<string>("User.LastName", "Smith");

        // Recoil.CreateAtom<T>(string: key, ISelectorBuilder:get)
        Username = Recoil.CreateSelector<string>("User.Username", GetUsername);
    }

    private static string GetUsername(ISelectorBuilder get)
        => $"{await get.GetAsync(FirstName)}_{await get.GetAsync(LastName)}";
}
```
Here we have defined two `Atom<T>` of state with default values. Later in this demo we will use these keys to get and fetch data. 

The more interesting one is the `Selector<string> Username`. Selectors are derived state and will update whenever one of it's dependencies changes values. In this case if we change the `FirstName` or `LastName` of the user the value of the `Username` property will be updated to reflect these changes. Selectors can also depend on other selectors allowing you to build complex objects dependency trees.  


### Reading Values

`Atom<T>` and `Selectors<T>` don't actually store any values themselves instead they are used as keys to access values in the `IRecoilStore` instance. To actually get the value we need to create some state. 

There is two ways of going about reading values from Recoil. 

### Code Behind 

You don't have to use View Models to use Recoil instead you can just bind data in the code behind.

First we have to define the properties that we want to use. We create a `RecoilState<T>` value holder for each property. The then initialize them in our constructor using the keys (Atom/Selector) that we crated in the first step.


```csharp
public class ProfileControl : UserControl 
{
    public RecoilState<string> Username { get; }
    public RecoilState<string> FirstName { get; }
    public RecoilState<string> LastName { get; } 

    public ProfileControl()
    {
        Username = this.UseRecoilState(UserState.Username);
        FirstName = this.UseRecoilState(UserState.LastName);
        LastName = this.UseRecoilState(UserState.LastName);

        // Initialize component must be called after creating the state
        // otherwise an exception will be thrown telling you to fix it. 
		InitializeComponent();
    }
}
```
You might have noticed that we don't need implement a `DependencyProperty` or the `INotifyPropertyChanged` interface. This is because each `RecoilState<T>` object already has it's own implementation. This is the reason `InitializeComponent()` has to be invoked last because if it was invoked before the `RecoilState<T>` objects would be null and no binding would be created.

After we have these setup we can then bind our data to our view 

```xml
<UserControl x:Class="ProfileControl">
    <StackPanel>
        <TextBlock>First Name:</TextBlock>
        <TextBox Text="{Binding FirstName.Value}"/>

        <TextBlock>Last Name:</TextBlock>
        <TextBox Text="{Binding LastName.Value}"/>

        <TextBlock Text="{Binding Username.Value, StringFormat=Hello {0}}">
    </StackPanel>
</UserControl>
```

Now whenever the user updates `FirstName` or `LastName` the text block will update with their username value.


### View Models 

To use a view model we use the extension method `UseRecoilViewModel<T>` of `FrameworkElement`.  

```xml 
public class ProfileControl : UserControl 
{
    public ProfileControl()
    {
        this.UseRecoilViewModel<ProfileViewModel>();
		InitializeComponent();
    }
}
```
This function does the following.
 1. Checks if the FrameworkElement is loaded
    1. No: go to step 2
    2. Yes: go to step 3
 2. Subscribes step 3 to `FrameworkElement.OnLoaded`.
 3. Walk the wpf hierarchy and finds the closes `RecoilRoot`
 4. Grabs the `RecoilRoot.Store`
 5. Invokes the factory method to produce the view model
 6. Assigns the `FrameworkElement.DataContext` value. 

All the being said is because the `DataContext` value won't always be read after invoking this method

Here is the definition of the view model with the required constructor.

```csharp
// The view model we want to bind data to
public class ProfileViewModel 
{
    public RecoilState<string> Username { get; }
    public RecoilState<string> FirstName { get; }
    public RecoilState<string> LastName { get; } 

    public ProfileViewModel(IRecoilStore store)
    {
        Username = store.UseState(UserState.Username);
        FirstName = store.UseState(UserState.FirstName);
        LastName = store.UseState(UserState.LastName);
    }
}
```
It's the same case with using code behind where we don't have to use `INotifyPropertyChanged` to get the values to update with binding.
