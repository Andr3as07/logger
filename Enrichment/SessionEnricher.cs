using System;
using System.Collections.Generic;

namespace Andr3as07.Logging.Enrichment {
  public class SessionEnricher : IEnricher {
    public static Guid UID = Guid.Empty;

    public void Enrich(Dictionary<string, object> properties, params object[] data) {
      if (UID == Guid.Empty) {
        UID = Guid.NewGuid();
      }

      properties["SessionGUID"] = UID.ToString();
    }
  }
}
