using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Andr3as07.Logging.Formater {
  public class JsonTextFormater : ITextFormater {
    private const int DefaultWriteBufferCapacity = 1024;

    private static string JsonEscape(string input) {
      return input.Replace("\"", "\\\"").Replace("\\", "\\\\");
    }

    public string FormatText(LogLevel level, DateTime time, string message, Dictionary<string, object> context) {
      StringBuilder builder = new StringBuilder(DefaultWriteBufferCapacity);
      builder.Append("{\"Timestamp\":").Append(new DateTimeOffset(time).ToUnixTimeMilliseconds());
      builder.Append(",\"Level\":\"").Append(level).Append("\",");
      builder.Append("\"Message\":\"").Append(JsonEscape(message)).Append("\"");
      if (context.Count > 0) {
        builder.Append(",\"Properties\":{");

        bool first = true;
        foreach (string key in context.Keys) {
          object value = context[key];
          string skey = key.Replace("\"", "\\\"");

          if (value == null) {
            continue;
          }

          if (!first) {
            builder.Append(",");
          }
          first = false;

          if (value is string svalue) {
            builder.Append("\"").Append(skey).Append("\":\"").Append(JsonEscape(svalue)).Append("\"");
          } else if (value is int) {
            builder.Append("\"").Append(skey).Append("\":").Append(value);
          } else if (value is float fvalue) {
            builder.Append("\"").Append(skey).Append("\":").Append(fvalue.ToString(CultureInfo.InvariantCulture));
          } else if (value is bool bvalue) {
            builder.Append("\"").Append(skey).Append("\":").Append(bvalue ? "true" : "false");
          } else {
            builder.Append("\"").Append(skey).Append("\":null"); // TODO: Handle more complex types
          }
        }

        builder.Append("}");
      }

      builder.Append("}");
      return builder.ToString();
    }

  }
}
