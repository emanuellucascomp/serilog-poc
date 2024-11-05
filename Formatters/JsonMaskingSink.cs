using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace WeatherApi.Formatters;

public class JsonMaskingSink(
    ITextFormatter formatter,
    Action<string> outputAction) : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        using var sw = new StringWriter();
        formatter.Format(logEvent, sw);
        var json = sw.ToString();
        var formattedJson = ApplyFormatting(json);
        outputAction(formattedJson);
    }

    private string ApplyFormatting(string json)
    {
        var jsonObj = ConvertToCamelCase(JObject.Parse(json));
        return jsonObj.ToString(Formatting.None);
    }

    private static JObject ConvertToCamelCase(JObject json)
    {
        var camelCaseResolver = new CamelCasePropertyNamesContractResolver();
        var camelCaseJson = new JObject();

        foreach (var property in json.Properties())
        {
            var camelCasePropertyName = camelCaseResolver.GetResolvedPropertyName(property.Name);
            camelCasePropertyName = CustomPropertyName(camelCasePropertyName);
            
            if (camelCasePropertyName == "timestamp" && DateTime.TryParse(property.Value.ToString(), out var timestamp))
            {
                camelCaseJson[camelCasePropertyName] = timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
            else if (property.Value is JObject jObject)
            {
                camelCaseJson[camelCasePropertyName] = ConvertToCamelCase(jObject);
            }
            else if (property.Value is JArray jArray)
            {
                var newArray = new JArray();
                foreach (var item in jArray)
                {
                    if (item is JObject arrayItemObject)
                    {
                        newArray.Add(ConvertToCamelCase(arrayItemObject));
                    }
                    else
                    {
                        newArray.Add(item);
                    }
                }
                camelCaseJson[camelCasePropertyName] = newArray;
            }
            else
            {
                camelCaseJson[camelCasePropertyName] = property.Value;
            }
        }
        return camelCaseJson;
    }

    private static string CustomPropertyName(string camelCasePropertyName)
    {
        if (camelCasePropertyName == "level")
        {
            camelCasePropertyName = "logLevel";
        }

        return camelCasePropertyName;
    }
}