using System;
using System.Collections.Generic;
using System.Text;

namespace Andr3as07.Logging.Sink {
  public class ColoredConsoleSink : ISink {
    private static readonly object Lock = new ColoredConsoleSink();

    private static readonly Dictionary<LogLevel, (ConsoleColor, ConsoleColor)> ColorCodes = new Dictionary<LogLevel, (ConsoleColor, ConsoleColor)>() {
      [LogLevel.Trace] = (ConsoleColor.DarkMagenta, ConsoleColor.Black),
      [LogLevel.Debug] = (ConsoleColor.Blue, ConsoleColor.Black),
      [LogLevel.Info] = (ConsoleColor.White, ConsoleColor.Black),
      [LogLevel.Warning] = (ConsoleColor.DarkYellow, ConsoleColor.Black),
      [LogLevel.Error] = (ConsoleColor.DarkRed, ConsoleColor.Black),
      [LogLevel.Critical] = (ConsoleColor.Red, ConsoleColor.Black),
      [LogLevel.Emergency] = (ConsoleColor.Yellow, ConsoleColor.Red),
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
