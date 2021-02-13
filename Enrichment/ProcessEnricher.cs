using System.Collections.Generic;
using System.Diagnostics;

namespace Andr3as07.Logging.Enrichment {
  public class ProcessEnricher : IEnricher {
    public void Enrich(Dictionary<string, object> properties, params object[] data) {
      Process current = Process.GetCurrentProcess();
      properties["ProcessId"] = current.Id;
      properties["ProcessName"] = current.ProcessName;
    }
  }
}
