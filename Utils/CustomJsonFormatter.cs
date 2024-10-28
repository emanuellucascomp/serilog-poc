namespace WeatherApi.Utils;

using Serilog.Events;
using Serilog.Formatting.Json;
using System.IO;

// public class CustomJsonFormatter : JsonFormatter
// {
//     public CustomJsonFormatter() : base(renderMessage: true)
//     {
//     }
//
//     protected override void WriteTimestamp(LogEvent logEvent, TextWriter output)
//     {
//         // Write "time_stamp" instead of "Timestamp"
//         output.Write(",\"time_stamp\":\"");
//         output.Write(logEvent.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
//         output.Write('\"');
//     }
// }