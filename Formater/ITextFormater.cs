using System;
using System.Collections.Generic;

namespace Andr3as07.Logging.Formater {
  public interface ITextFormater {
    string FormatText(LogLevel level, DateTime time, string message, Dictionary<string, object> context);
  }
}
