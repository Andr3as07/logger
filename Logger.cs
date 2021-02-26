using Andr3as07.Logging.Enrichment;
using Andr3as07.Logging.Sink;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Andr3as07.Logging {

  public class Logger : ISink, IDisposable {
    public List<ISink> Sinks;
    public List<IEnricher> Enrichers;
    public Dictionary<string, object> Properties;
    public LogLevel Level;

    public Logger(LogLevel level) {
      Sinks = new List<ISink>();
      Enrichers = new List<IEnricher>();
      Properties = new Dictionary<string, object>();
      Level = level;
    }

    public void Log(LogLevel level, string message, params object[] data) {
      if (level < Level) {
        return;
      }

      DateTime time = DateTime.UtcNow;

      // Create a copy of the properties to protect against enrichers modifing constant properties.
      Dictionary<string, object> context = Properties.ToDictionary(entry => entry.Key,
                                                                   entry => entry.Value);

      if (data.Length >= 1) {
        // Merge old context
        if (data[0] is Dictionary<string, object> dictionary) {
          // Merge contexts
          dictionary.ToList().ForEach(x => context[x.Key] = x.Value);

          // Remove first element
          data = data.Skip(1).ToArray();
        }

        // Resolve data
        for (int i = 0; i < data.Length; i++) {
#if !(NETFRAMEWORK || NETSTANDARD)
          if (data[i] is ITuple dataEntry1) {
            context[(string)dataEntry1[0]] = dataEntry1[1];
          } else
#endif
          if (data[i] is Tuple<string, object> dataEntry2) {
            context[dataEntry2.Item1] = dataEntry2.Item2;
          }
        }
      }

      // Run Enrichers
      foreach (IEnricher enricher in Enrichers) {
        enricher.Enrich(context, data);
      }

      // Output to sinks
      foreach (ISink sink in Sinks) {
        sink.Dispatch(level, time, message, context, data);
      }
    }

    public void Trace(string message, params object[] data) {
      Log(LogLevel.Trace, message, data);
    }

    public void Debug(string message, params object[] data) {
      Log(LogLevel.Debug, message, data);
    }

    public void Info(string message, params object[] data) {
      Log(LogLevel.Info, message, data);
    }

    public void Warn(string message, params object[] data) {
      Log(LogLevel.Warning, message, data);
    }

    public void Error(string message, params object[] data) {
      Log(LogLevel.Error, message, data);
    }

    public void Critical(string message, params object[] data) {
      Log(LogLevel.Critical, message, data);
    }

    public void Emergency(string message, params object[] data) {
      Log(LogLevel.Emergency, message, data);
    }

    public virtual void Flush() {
      foreach (ISink sink in Sinks) {
        (sink as IFlushable)?.Flush();
      }
    }

    public void Dispatch(LogLevel level, DateTime time, string message, Dictionary<string, object> context, params object[] data) {
      Log(level, message, context, data);
    }

    public void Dispose() {
      foreach (ISink sink in Sinks) {
        (sink as IDisposable)?.Dispose();
      }

      foreach (IEnricher enricher in Enrichers) {
        (enricher as IDisposable)?.Dispose();
      }
    }
  }
}
