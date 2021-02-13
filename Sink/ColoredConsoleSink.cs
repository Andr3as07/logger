using System;
using System.Collections.Generic;
using System.Text;

namespace Andr3as07.Logging.Sink {
  public class ColoredConsoleSink : ISink {
    private static readonly object Lock = new ColoredConsoleSink();

    private static readonly Dictionary<LogLevel, (ConsoleColor, ConsoleColor)> ColorCodes = new Dictionary<LogLevel, (ConsoleColor, ConsoleColor)>() {
      [LogLevel.TRACE] = (ConsoleColor.DarkMagenta, ConsoleColor.Black),
      [LogLevel.DEBUG] = (ConsoleColor.Blue, ConsoleColor.Black),
      [LogLevel.INFO] = (ConsoleColor.White, ConsoleColor.Black),
      [LogLevel.WARNING] = (ConsoleColor.DarkYellow, ConsoleColor.Black),
      [LogLevel.ERROR] = (ConsoleColor.DarkRed, ConsoleColor.Black),
      [LogLevel.CRITICAL] = (ConsoleColor.Red, ConsoleColor.Black),
      [LogLevel.EMERGENCY] = (ConsoleColor.Yellow, ConsoleColor.Red),
    };

    public ColoredConsoleSink() {
    }

    public void Dispatch(LogLevel level, DateTime time, string message, Dictionary<string, object> context, params object[] data) {
      lock (Lock) {
        Console.ForegroundColor = ColorCodes[level].Item1;
        Console.BackgroundColor = ColorCodes[level].Item2;
        Console.Write($"{level}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Write($": {message}\n");
      }
    }
  }
}
