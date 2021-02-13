using Andr3as07.Logging.Formater;

using System;
using System.Collections.Generic;

namespace Andr3as07.Logging.Sink {
  public class DiagnosticSink : ISink {
    public readonly ITextFormater Formater;

    public DiagnosticSink(ITextFormater formater) {
      this.Formater = formater;
    }

    public void Dispatch(LogLevel level, DateTime time, string message, Dictionary<string, object> context, params object[] data) {
      string str = Formater.FormatText(level, time, message, context);
      switch(level) {
        case LogLevel.TRACE:
          System.Diagnostics.Trace.WriteLine(str);
          break;
        case LogLevel.DEBUG:
        case LogLevel.INFO:
        case LogLevel.WARNING:
        case LogLevel.ERROR:
        case LogLevel.CRITICAL:
        case LogLevel.EMERGENCY:
          System.Diagnostics.Debug.WriteLine(str);
          break;
        default:
          // TODO: Self logging
          System.Diagnostics.Debug.WriteLine(str);
          break;
      }
    }
  }
}
