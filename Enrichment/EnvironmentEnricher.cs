using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Andr3as07.Logging.Enrichment {
  public class EnvironmentEnricher : IEnricher {

    [Flags]
    public enum EnvironmentScope {
      None = 0x00,

      MachineName = 0x01,
      Username = 0x02,
      Envionment = 0x04,

      All = MachineName | Username | Envionment
    }

    public readonly EnvironmentScope Scope;

    public EnvironmentEnricher(EnvironmentScope scope = EnvironmentScope.All) {
      this.Scope = scope;
    }

    private static string GetMachineName() {
      string name = Environment.MachineName;

      if (string.IsNullOrEmpty(name)) {
        name = System.Net.Dns.GetHostName();
      }

      if (string.IsNullOrEmpty(name)) {
        name = Environment.GetEnvironmentVariable("COMPUTERNAME");
      }

      return name;
    }

    private static string GetUsername() {
      string name = Environment.UserName;

      if (string.IsNullOrEmpty(name)) {
        name = WindowsIdentity.GetCurrent().Name;
      }

      if (string.IsNullOrEmpty(name)) {
        name = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
      }

      return name;
    }

    private static string GetEnvironment() {
      string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

      if (string.IsNullOrEmpty(env)) {
        env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
      }

      return env;
    }

    public void Enrich(Dictionary<string, object> properties, params object[] data) {
      if(Scope.HasFlag(EnvironmentScope.MachineName)) {
        properties["EnvironmentMachineName"] = GetMachineName();
      }

      if (Scope.HasFlag(EnvironmentScope.Username)) {
        properties["EnvironmentUserName"] = GetUsername();
      }

      if (Scope.HasFlag(EnvironmentScope.Envionment)) {
        properties["Environment"] = GetEnvironment();
      }
    }
  }
}
