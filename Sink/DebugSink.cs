using Andr3as07.Logging.Formater;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Andr3as07.Logging.Sink {
  public class DebugSink : ISink {
    public readonly ITextFormater Formater;

    public DebugSink(ITextFormater formater) {
      this.Formater = formater;
    }

    public void Dispatch(LogLevel level, DateTime time, string message, Dictionary<string, object> context, params object[] data) {
      string str = Formater.FormatText(level, time, message, context);
      Debug.WriteLine(str);
    }
  }
}
