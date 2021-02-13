using System;
using System.Collections.Generic;
using System.Text;

namespace Andr3as07.Logging.Sink {
  public interface ISink {
    void Dispatch(LogLevel level, DateTime time, string message, Dictionary<string, object> context, params object[] data);
  }
}
