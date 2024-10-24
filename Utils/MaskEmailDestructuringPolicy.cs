namespace WeatherApi.Utils;

using Serilog.Core;
using Serilog.Events;
using System.Linq;

public class MaskEmailEnricher : ILogEventEnricher
{
    // List of known field names where email addresses might exist
    private static readonly string[] EmailFieldNames = { "Email", "email", "EmailAddress", "emailAddress" };

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // Check each property in the log event and mask email fields
        var updatedProperties = logEvent.Properties.ToDictionary(
            kvp => kvp.Key,
            kvp => MaskEmailIfNeeded
                (kvp.Key, kvp.Value)
        );

        // Replace the original properties with the masked properties
        foreach (var updatedProperty in updatedProperties)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty(updatedProperty.Key, updatedProperty.Value));
        }
    }

    // This method masks the email if the key or value indicates it is an email field
    private LogEventPropertyValue MaskEmailIfNeeded(string key, LogEventPropertyValue value)
    {
        // Check if the key is an email-related field or if the value contains an email
        if (EmailFieldNames.Contains(key) && value is ScalarValue scalar && scalar.Value is string str && str.Contains("@"))
        {
            return new ScalarValue(MaskEmail(str));
        }

        // Recursively check nested objects (structured values)
        if (value is StructureValue structure)
        {
            var maskedProperties = structure.Properties
                .Select(p => new LogEventProperty(p.Name, MaskEmailIfNeeded(p.Name, p.Value)))
                .ToList();
            return new StructureValue(maskedProperties);
        }

        // If the value is a sequence (array), mask emails inside it
        if (value is SequenceValue sequence)
        {
            var maskedSequence = sequence.Elements
                .Select(v => MaskEmailIfNeeded(key, v))
                .ToList();
            return new SequenceValue(maskedSequence);
        }

        return value; // No email detected, return original value
    }

    // Utility to mask the email by replacing the part before '@'
    private string MaskEmail(string email)
    {
        var atIndex = email.IndexOf('@');
        return atIndex > 0 ? "*****" + email.Substring(atIndex) : email;
    }
}