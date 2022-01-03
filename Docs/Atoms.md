# Atoms 

Atoms are used as a key used to access a values in Recoil. Similar to [Selectors]('./../Selectors.md) they require a unique string key which is used to store the value. This key must be unique across the whole application otherwise an exception will be thrown.

Atoms should be static and accessible to the whole application. 

```cs
public static class AppState
{
    public static readonly Atom<string> FirstName;

    static AppState
    {
        FirstName = new Atom<string>("AppState.FirstName");
    }
}
```

## Default Value 

Atoms themselves don't store any values but are instead used to fetch a value from the store. If the store does not contain a defined value the default will be used. There is three different default values that can be used. 

### Constant Value

```cs
Atom<string> userName = new Atom<string>("UserName", "jsmith");
```
The simplest form is a constant value which has no magic behind it. 

### Atom Value

```cs
// Use a common value for the default
Atom<string> unsetValue = new Atom<string>("Default.UnsetValue", "Unknown");

// Both atoms default to 'Unknown'
Atom<string> firstName = new Atom<string>("User.FirstName", unsetValue);
Atom<string> lastName = new Atom<string>("User.LastName", unsetValue);
```

This might seem a little weird to use an Atom instead of a constant but stay with me. 
Lets say you are writing an application that also works in French. We can set the `unsetValue` to `inconnue` for French. Then every single place across the application that is using `usetValue` as the default will update but only if the Atom they are defaulting for does not already have a value. 

### Selector Value 

Selectors work very much in the same way as Atom's for default values. Since selectors are accessed in the exact same way the have the same behaviour.
