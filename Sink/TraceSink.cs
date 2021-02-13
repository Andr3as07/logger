using Andr3as07.Logging.Formater;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Andr3as07.Logging.Sink {
  public class TraceSink : ISink {
    public readonly ITextFormater Formater;

    public TraceSink(ITextFormater formater) {
      this.Formater = formater;
    }

    public void Dispatch(LogLevel level, DateTime time, string message, Dictionary<string, object> context, params object[] data) {
      string str = Formater.FormatText(level, time, message, context);
      switch(level) {
        case LogLevel.ERROR:
        case LogLevel.CRITICAL:
        case LogLevel.EMERGENCY:
          Trace.TraceError(str);
          break;

        case LogLevel.WARNING:
          Trace.TraceWarning(str);
          break;

        case LogLevel.INFO:
          Trace.TraceInformation(str);
          break;

        case LogLevel.TRACE:
        case LogLevel.DEBUG:
          Debug.WriteLine(str);
          break;

        default:
          // TODO: Self logging
          Debug.WriteLine(str);
          break;
      }
    }
  }
}
