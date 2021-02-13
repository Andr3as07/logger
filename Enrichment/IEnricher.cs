using System.Collections.Generic;

namespace Andr3as07.Logging.Enrichment {
  public interface IEnricher {
    void Enrich(Dictionary<string, object> properties, params object[] data);
  }
}
