using Andr3as07.Logging.Formater;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Andr3as07.Logging.Sink {
  public class FileSink : ISink, IDisposable, IFlushable {
    public enum RollingPolicy {
      None = 0x00,

      Daily = 0x01,
      Weekly = 0x02,
      Monthly = 0x03,
      Yearly = 0x04
    }

    public readonly string Path;
    public readonly ITextFormater Formater;
    public readonly long FileSizeLimit; 
    public readonly RollingPolicy Rolling;

    private StreamWriter writer;
    private string currentPath = null;

    public FileSink(string path, ITextFormater formater, RollingPolicy rolling=RollingPolicy.None, long fileSizeLimit =1024*1024*1024 /* 1GB */) {
      this.Path = path;
      this.Formater = formater;
      this.Rolling = rolling;
      this.FileSizeLimit = fileSizeLimit;
    }

    public void Dispose() {
      if (writer != null) {
        writer.Close();
        writer.Dispose();
        writer = null;
      }

      if (Formater != null) {
        (Formater as IDisposable)?.Dispose();
      }
    }

    private StreamWriter GetWriter(DateTime time) {
      string path = System.IO.Path.GetFullPath(Path);
      string dir = System.IO.Path.GetDirectoryName(path);
      if(Rolling != RollingPolicy.None) {
        string ext = System.IO.Path.GetExtension(path);
        string fname = System.IO.Path.GetFileNameWithoutExtension(path);
        string dateString = null;
        if (Rolling == RollingPolicy.Daily) {
          dateString = time.ToString("yyyyMMdd");
        } else if (Rolling == RollingPolicy.Weekly) {
          string weekNum = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
              time, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString("00");
          dateString = time.ToString("yyyy") + "w" + weekNum;
        } else if (Rolling == RollingPolicy.Monthly) {
          dateString = time.ToString("yyyy") + "m" + time.Month.ToString("00");
        } else if (Rolling == RollingPolicy.Yearly) {
          dateString = time.ToString("yyyy");
        }

        path = System.IO.Path.Combine(dir, $"{fname}{dateString}{ext}");
      }

      if (currentPath != path && writer != null) {
        writer.Flush();
        writer.Dispose();
      }

      if(!Directory.Exists(dir)) {
        Directory.CreateDirectory(dir);
      } else if(File.Exists(path)) {
        if(new FileInfo(path).Length > FileSizeLimit) {
          // TODO: Self logging
          return null; // TODO: Roll over to the next file
        }
      }

      if (writer == null) {
        if (!File.Exists(path)) {
          writer = File.CreateText(path);
          currentPath = path;
        } else {
          writer = File.AppendText(path);
          currentPath = path;
        }
      }

      return writer;
    }

    public void Dispatch(LogLevel level, DateTime time, string message, Dictionary<string, object> context, params object[] data) {
      GetWriter(time)?.WriteLine(Formater.FormatText(level, time, message, context));
    }

    public void Flush() {
      if (writer == null) {
        return;
      }

      writer.Flush();
    }
  }
}
