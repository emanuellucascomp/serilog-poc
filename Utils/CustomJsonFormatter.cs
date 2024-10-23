namespace WeatherApi.Utils;

using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;

public class CustomJsonTextFormatter : ITextFormatter
{
    private readonly string _timestampFormat;

    public CustomJsonTextFormatter(string timestampFormat = "dd/MM/yyyy HH:mm:ss.fff")
    {
        _timestampFormat = timestampFormat;
    }

    public void Format(LogEvent logEvent, TextWriter output)
    {
        output.Write("{");

        // Write timestamp
        output.Write("\"timestamp\":\"");
        output.Write(logEvent.Timestamp.ToString(_timestampFormat));
        output.Write("\",");

        // Write log level
        output.Write("\"logLeve\":\"");
        output.Write(logEvent.Level);
        output.Write("\",");

        // Write message template
        output.Write("\"messageTemplate\":\"");
        output.Write(logEvent.MessageTemplate.Text);
        output.Write("\",");

        // Write rendered message
        output.Write("\"message\":\"");
        output.Write(logEvent.RenderMessage());
        output.Write("\",");

        // Write log properties (if any)
        output.Write("\"properties\":{");
        foreach (var property in logEvent.Properties)
        {
            output.Write($"\"{property.Key.ToLower()}\":");
            property.Value.Render(output);
            output.Write(",");
        }

        output.Write("}}");
        output.WriteLine();
    }
}