using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Andr3as07.Logging.Formater {
  public class XmlTextFormater : ITextFormater {
    private const int DefaultWriteBufferCapacity = 1024;

    private string XmlEscape(string inString) {
      return inString.Replace("\"", "&quot;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
    }

    public string FormatText(LogLevel level, DateTime time, string message, Dictionary<string, object> context) {
      StringBuilder builder = new StringBuilder(DefaultWriteBufferCapacity);
      builder.Append("<LogEntry>");
      builder.Append("<Timestamp>").Append(new DateTimeOffset(time).ToUnixTimeMilliseconds()).Append("</Timestamp>");
      builder.Append("<Level>").Append(level).Append("</Level>");
      builder.Append("<Message>").Append(XmlEscape(message)).Append("</Message>");
      if (context.Count > 0) {
        builder.Append("<Properties>");

        foreach (string key in context.Keys) {
          object value = context[key];

          if (value == null) {
            continue;
          }

          builder.Append("<").Append(key).Append(">");
          if (value is string svalue) {
            builder.Append(XmlEscape(svalue));
          } else if (value is int) {
            builder.Append(value);
          } else if (value is float fvalue) {
            builder.Append(fvalue.ToString(CultureInfo.InvariantCulture));
          } else if (value is bool bvalue) {
            builder.Append(bvalue ? "true" : "false");
          } else {
            // TODO: Handle more complex types
          }
          builder.Append("</").Append(key).Append(">");
        }

        builder.Append("</Properties>");
      }

      builder.Append("</LogEntry>");
      return builder.ToString();
    }

  }
}
