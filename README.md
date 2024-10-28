# Custom Serilog Logging Project

This project demonstrates how to customize Serilog's JSON log output. Using a custom JSON formatter, we override Serilog’s default serialization behavior, allowing us to output logs with customized field names and formats.

# Problems

- Inherit from JsonFormatter is impossible because it's a sealed class.
- To change the names of fields outside of the Properties property (e.g., Timestamp, Level, and other top-level fields), an enricher alone won’t be enough, as enrichers are typically used to add or modify fields within the Properties dictionary. However, you can achieve the renaming by combining a custom JSON formatter with an enricher. The custom formatter will allow us to modify how fields like Timestamp are serialized, while the enricher can be used to add any additional fields or transformations to the log event as needed.

# Which Approach Is Best?

Custom JSON Formatter (Approach 1) is the most direct and flexible if you're looking to customize the serialization format of Serilog log events.

Custom Enricher (Approach 2) is useful if you want to add custom fields but not necessarily suppress existing ones like Timestamp.

# TODO

- Customization with JsonNode and Newtonsoft
- https://learn.microsoft.com/en-us/dotnet/api/system.text.json?view=net-8.0