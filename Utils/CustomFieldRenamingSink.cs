using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace WeatherApi.Utils;

using Serilog.Core;
using Serilog.Events;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;

public class CustomFieldRenamingSink : ILogEventSink
{
    private readonly TextWriter _output;

    public CustomFieldRenamingSink(TextWriter output)
    {
        _output = output;
    }

    public void Emit(LogEvent logEvent)
    {
        var logDictionary = new Dictionary<string, object>
        {
            // Rename "Timestamp" to "time_stamp"
            { "timestamp", logEvent.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
            // Rename "Level" to "log_level"
            { "logLevel", logEvent.Level.ToString() },
            // Rename "MessageTemplate" to "template"
            { "template", logEvent.MessageTemplate.Text },
            // Rename "RenderedMessage" to "message"
            { "message", logEvent.RenderMessage() }
        };

        // Add all properties under "Properties" as usual
        foreach (var property in logEvent.Properties)
        {
            logDictionary[property.Key] = TrimQuotesFromValue(property.Value.ToString());
        }
        
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // This avoids Unicode escaping
        };

        // Serialize the modified dictionary to JSON
        string json = JsonSerializer.Serialize(logDictionary, jsonOptions);
        json = json.Replace("\\\"", "\"");
        
        // Write to the specified output (e.g., console)
        _output.WriteLine(json);
    }
    
    private string TrimQuotesFromValue(string value)
    {
        // Removes leading and trailing quotes from string values
        if (value.StartsWith("\"") && value.EndsWith("\""))
        {
            return value.Substring(1, value.Length - 2);
        }
        return value;
    }
}