using Andr3as07.Logging.Formater;

using System;
using System.Collections.Generic;
using System.Text;

namespace Andr3as07.Logging.Sink {
  public class ConsoleSink : ISink {
    public readonly ITextFormater Formater;

    public ConsoleSink(ITextFormater formater) {
      this.Formater = formater;
    }

    public void Dispatch(LogLevel level, DateTime time, string message, Dictionary<string, object> context, params object[] data) {
      Console.WriteLine(Formater.FormatText(level, time, message, context));
    }
  }
}
