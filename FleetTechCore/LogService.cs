using System.Text.Json;

namespace FleetTechCore;

public static class LogService {
   public static void SetLogPath(string? path)
   { 
      logPath = path ?? "C:\\BFv2\\Logs";
   }
   private static string logPath { get; set; }
   private static readonly string nl = Environment.NewLine;
   
   private static string LogPath(string? directory, string filename, DateTime? date = null) {
      if (date is null)
         Directory.CreateDirectory(directory is null? logPath : logPath + $"\\{directory}");
      else Directory.CreateDirectory((directory is null? logPath : logPath + $"\\{directory}") + $"\\{date.Value.Date.Year}\\{date.Value.Date.Month}\\{date.Value.Date.Day}");
      return Path.Combine((directory is null ? logPath : logPath + $"\\{directory}") + (date is null ? string.Empty : $"\\{date.Value.Date.Year}\\{date.Value.Date.Month}\\{date.Value.Date.Day}"), filename);
   }
   public static void LogException(Exception exception, string? directory = null, DateTime? date = null) {
      date ??= DateTime.Now;
      Task.Run(() => {
         try {
            File.WriteAllText(LogPath(directory + "\\Errors", $"{date.Value.Ticks}_{exception.GetType().Name}.error-log", date), 
               string.Join(nl, 
                  $"Time: {date:yyyy-MM-dd HH:mm:ss.fff}", 
                  Extensions.ExceptionToString(exception)));
         } catch (Exception ex) { ; }
      });
   }

   public static void LogServiceResponse<T>(T objectToLog, string postMessage, string response, DateTime date, string? serviceName = null) where T : class {
      Task.Run(() => {
         try {
            var true_date = DateTime.Now;
            File.WriteAllText(LogPath(serviceName,$"{date:yyyy-MM-dd}_{serviceName}.service-log", date), 
               string.Join(nl,
                  $"Time: {true_date:yyyy-MM-dd HH:mm:ss.fff}",
                  $"Service: {serviceName}",
                  $"Service Args: {date:yyyy-MM-dd}", 
                  $"Data: {JsonSerializer.Serialize(objectToLog)}",
                  $"Post Message: {postMessage}",
                  $"Response: {response}"
               ));
         } catch (Exception ex) { LogException(ex, serviceName, date); }
      });
   }
}
