# NConsole.Options.Extensions

Ever thought so much verbiage could be said about a command line argument? Actually,
it's not a slam dunk, by any stretch of the imagination, and can be made a bit easier
to contend with. That is the purpose of this library, to facilitate easier exposure of
command line arguments.

## Code Organization

In general, the library itself depends on there being an [OptionSet](https://github.com/mwpowellhtx/NConsole.Options/blob/master/src/NConsole.Options/OptionSet.cs)
instance available. The library does not enforce ways for you to organize your code.
However, I would strongly encourage you to adopt a [S.O.L.I.D.](http://en.wikipedia.org/wiki/SOLID_%28object-oriented_design%29),
[D.R.Y.](http://en.wikipedia.org/wiki/Don%27t_repeat_yourself) approach. Which means
you necessarily separate the concerns of options, parsing, etc, from the rest of your
application. However, this topic is beyond the scope of this repository.

## Driving Motivations

The chief driving motivation for me was to deliver this into a [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
environment.

## The Basics

This library is exactly what it says it is: an Extension to the [NConsole.Options](https://github.com/mwpowellhtx/NConsole.Options)
library. So if you are unfamiliar
with Options, you should get familiar with that first. You can do that by installing
Extensions, or just run with Options, at your discretion.

Basically, Extensions wraps the Options in a friendlier, at least in my opinion,
fluent-style variable-completion for command-line options.

## OptionSet and RequiredValuesOptionSet

Extensions builds upon the (by now) familiar [OptionSet](https://github.com/mwpowellhtx/NConsole.Options/blob/master/src/NConsole.Options/OptionSet.cs)
in its purest form. Additionally, it is possible to add required values using the
[RequiredValuesOptionSet](https://github.com/mwpowellhtx/NConsole.Options.Extensions/blob/master/src/NConsole.Options.Extensions/RequiredValuesOptionSet.cs)
Extension to [OptionSet](https://github.com/mwpowellhtx/NConsole.Options/blob/master/src/NConsole.Options/OptionSet.cs).

## Adding Variables

There are several ways to add variables to support simple variables. Let's assume that
we have the following [OptionSet](https://github.com/mwpowellhtx/NConsole.Options/blob/master/src/NConsole.Options/OptionSet.cs)
available:

```C#
var options = new OptionSet();
```

### Switch

*Switch* is a simple command line class that says whether it is Enabled. Enabled means
that the switch was specified at the command line.

```C#
var option = options.AddSwitch("Option", "Switch to set");
// Parse the arguments...
if (option.Enabled)
{
    // Do something...
}
```

For convenience, Switch implicitly converts to bool:

```C#
if (option)
{
    // Do something...
}
```

### Variable

A *Variable* can also have a built-in type associated with it.

```C#
var option = options.AddVariable<int>("Option", "Value to set");
// Parse the arguments...
if (option.Value > 0)
{
    // Do something...
}
```

For convenience, Variable implicitly converts to its underlying Value.

### VariableList

A *VariableList* can be requested and is simply a name associated with a list of values.
For as many times as the name/value appears, you will have those values enumerated in
the variable when you work with it.

```C#
var timeouts = options.AddVariableList<int>("Timeout", "Timeout in milliseconds");
// Parse the arguments...
foreach (var timeout in timeouts.Values)
{
    // Do something with timeout...
}
```

For convenience, VariableList also looks-and-feels like an IEnumerable:

```C#
foreach (var timeout in timeouts)
{
    // ...
}
```

### VariableMatrix

Last but not least, *VariableMatrix* supports cataloging a dictionary of name/value
pairs associated with a command line argument. At the command line these appear like:

```
... -n:Name=Value -n:Name2=Value2 "-n:Name3=Value3 Value4 Value5"
```

Or such as this at the language level:

```C#
var args = new string[]
{
    "-n:Name=Value",
    "-n:Name2=Value2",
    "-n:Name3=Value3 Value4 Value5",
};
```

Which parses to:

```C#
IDictionary<string, string> parsed = new Dictionary<string, string>()
{
    {"Name", "Value"},
    {"Name2", "Value2"},
    {"Name3", "Value3 Value4 Value5"}
};
```

## Examples

Okay, now for the code example:

```C#
var option = options.AddVariableMatrix<int>("Option", "Matrix option values");
// Parse the arguments...
foreach (var key in option.Matrix.Keys)
{
    // Do something with option[key]...
}
```

For convenience, VariableMatrix also looks-and-feels like an IDictionary whose key
is string and whose value is the specified type.

```C#
foreach (var key in option)
{
    // ...
}
```

**Note**: Microsoft .NET Framework 4.5 introduces an IReadOnlyDictionary concept,
which would be perfect for this use-case. However, since we are supporting backwards
compatibility, we will need to overlook that usefulness. It's a documented issue in
the repository, and I may take a gander at how better to migrate into a purely
read-only use-case. For now, it is left to end-user discipline not to mutate the
matrix dictionary.

## Parsing Arguments

Arguments are passed through the Program Main method as per usual. For instance:

```C#
public static void Main(params string[] args)
{
    // ...
}
```

A convenience controller has been provided, ConsoleManager, which may be used
as follows:

```C#
var options = new RequiredValuesOptionSet();

using (var consoleManager = new ConsoleManager("My Console", options))
{
    var writer = new TextWriter();
    if (consoleManager.TryParseOrShowHelp(writer, args))
    {
        // TODO: Do something with the variables...
    }
}

```

Standard Extension parsing functionality writes a usage blurb including the ConsoleName,
passed to the ConsoleManager ctor, as well as option descriptions. This occurs when
parsing is incomplete or in error for any reason.

In production code, any TextWriter will do, but you probably want to interact with the
Console for your output. Straightforward enough:

```C#
if (cm.TryParseOrShowHelp(Console.Out, args))
{
    // ...
}
```

There you go. I practically handed you your production usage right there.

## Roadmap

I welcome ideas and ways to improve upon the extensibility of
[NConsole.Options.Extensions](https://github.com/mwpowellhtx/NConsole.Options.Extensions).
At present, plausible areas of extension and/or improvement include, but are not limited to:

- [ ] In its present condition, I am of the opinion that [OptionSet](https://github.com/mwpowellhtx/NConsole.Options/blob/master/src/NConsole.Options/OptionSet.cs)
should be supporting the behavior introduced by [RequiredValuesOptionSet](https://github.com/mwpowellhtx/NConsole.Options.Extensions/blob/master/src/NConsole.Options.Extensions/RequiredValuesOptionSet.cs),
especially considering the existence of [OptionValueType](https://github.com/mwpowellhtx/NConsole.Options/blob/master/src/NConsole.Options/OptionValueType.cs).

- [ ] I am considering refactoring strategic domain level constant definitions to
the [NConsole.Options](https://github.com/mwpowellhtx/NConsole.Options) API as well.
Especially in areas where we are basically repeating ourselves from the root source
code.

- [ ] I am pretty confident with the conferred type conversion provided by
[NConsole.Options](https://github.com/mwpowellhtx/NConsole.Options) at this point.
I am of the mindset that less of this is necessary, if at all, from an
[NConsole.Options.Extensions](https://github.com/mwpowellhtx/NConsole.Options.Extensions) perspective.
