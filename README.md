# Andr3as07's Logging Framework
This project is a diagnostic logging library for .NET applications. It is focused on ease of use and runs on all recent .NET platforms. It provides structured logging and allows for complex distributed applications and systems.

The Logging Frameword provides a simple to use interface.
```csharp
Logger logger = new Logger(LogLevel.Info);
logger.Sinks.Add(new ConsoleSink(new SimpleTextFormater()));

logger.Info("It Works!");
```

Logging Framework is designed for *structured logging*.
```csharp
logger.Debug($"Request to {url} took {request.time}ms.", ("URL", url), ("RequestDuration", request.time));
```

The Framework interpretes these values and generates a Log Event that gets handled by the Sink,
which usualy asks a provided Formater to turn it either into a string or binary blob,
and written to disk, displayed to the user, logged in the debug console, sent to your logging server, or written to a database.

## Getting started
Download an compile the repository. You can now choose the required .dll file form the build directory.

The simplest way to set up the logging framework is using a static Logger instance.

```csharp
using Andr3as07.Logging;

public class Program {
    public static Logger logger;

    public static void Main() {
        logger = new Logger(LogLevel.Info);
        logger.Sinks.Add(new ConsoleSink(new SimpleTextFormater()));
        logger.Sinks.Add(new FileSink("log.json", new JsonTextFormater(), RollingPolicy.Daily));
            
        logger.Info("Hello, World!");
        
        logger.Flush();
        logger.Dispose();
    }
}
```

## Sinks
Sinks are the destination to which the log entry should be written.
A sink could be anything:
* The Console Window
* A file on disk
* The windows event log
* An external logging service
* Anything else you can come up with

## Formaters
A Formater takes the 