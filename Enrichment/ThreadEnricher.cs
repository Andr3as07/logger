using System.Collections.Generic;
using System.Threading;

namespace Andr3as07.Logging.Enrichment {
  public class ThreadEnricher : IEnricher {
    public void Enrich(Dictionary<string, object> properties, params object[] data) {
      properties["ThreadId"] = Thread.CurrentThread.ManagedThreadId;
      properties["ThreadName"] = Thread.CurrentThread.Name;
      properties["ThreadPriority"] = Thread.CurrentThread.Priority.ToString();
    }
  }
}
