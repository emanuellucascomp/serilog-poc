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
            { "timeStamp", logEvent.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
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
            logDictionary[property.Key] = property.Value.ToString();
        }

        // Serialize the modified dictionary to JSON
        string json = JsonSerializer.Serialize(logDictionary);
        
        // Write to the specified output (e.g., console)
        _output.WriteLine(json);
    }
}