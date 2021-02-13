using System;
using System.Collections.Generic;

namespace Andr3as07.Logging.Formater {
  public class SimpleTextFormater : ITextFormater {
    public readonly string TimeFormat;

    public SimpleTextFormater(string timeFormat = "yyyy-MM-dd HH:mm:ss:fff") {
      this.TimeFormat = timeFormat;
    }

    public string FormatText(LogLevel level, DateTime time, string message, Dictionary<string, object> context) {
      if (!string.IsNullOrEmpty(TimeFormat)) {
        string timeStr = time.ToString(TimeFormat);
        return $"{timeStr} {level}: {message}";
      }

      return "{level}: {message}";
    }
  }
}
