# Console in Docker - A library helping .NET developers to build console applications in Docker

[![Build status](https://ci.appveyor.com/api/projects/status/u0485194qofix1w3/branch/master?svg=true)](https://ci.appveyor.com/project/FantasticFiasco/console-in-docker/branch/master)
[![NuGet Version](http://img.shields.io/nuget/v/ConsoleInDocker.svg?style=flat)](https://www.nuget.org/packages/ConsoleInDocker/) 

__Package__ - [ConsoleInDocker](https://www.nuget.org/packages/consoleindocker)
| __Platforms__ - .NET Standard 2.0

## Super simple to use

In the following example, the console application will not terminate until Docker tells it to, or Ctrl+C is pressed.

```csharp
public class Program
{
  public static void Main()
  {
    Console.WriteLine("Hello world!");

    Wait.ForShutdown();
  }
}
```

