# Custom Serilog Logging Project

This project demonstrates how to customize Serilog's JSON log output. Using a custom JSON formatter, we override Serilog’s default serialization behavior, allowing us to output logs with customized field names and formats.

# Problems

- Inherit from JsonFormatter is impossible because it's a sealed class.
- To change the names of fields outside of the Properties property (e.g., Timestamp, Level, and other top-level fields), an enricher alone won’t be enough, as enrichers are typically used to add or modify fields within the Properties dictionary. However, you can achieve the renaming by combining a custom JSON formatter with an enricher. The custom formatter will allow us to modify how fields like Timestamp are serialized, while the enricher can be used to add any additional fields or transformations to the log event as needed.

# Which Approach Is Best?

Custom JSON Formatter (Approach 1) is the most direct and flexible if you're looking to customize the serialization format of Serilog log events.

Custom Enricher (Approach 2) is useful if you want to add custom fields but not necessarily suppress existing ones like Timestamp.

# CustomJsonTextFormatter

## Pros:

### Direct Modification of JSON Output:
- Allows fine-grained control over the output of each individual field by extending JsonFormatter. Each top-level field (Timestamp, Level, etc.) can be renamed as needed before being output.

### Efficiency:
- Since this method directly formats the JSON output, it can be faster and more memory-efficient compared to capturing and re-processing the entire log event.

### Integrated in Log Pipeline:
- By using a formatter, you stay within Serilog's core pipeline without needing an external layer for transformation, making it easier to maintain and configure.

## Compatibility with Standard Sinks:
- Works well with standard Serilog sinks like Console, File, and others that accept custom formatters, making it flexible for different logging targets.

## Cons:

### Limited to JSON Output:
- This approach is only effective for sinks that accept ITextFormatter-based formatting, such as Console and File. It won’t work with all sinks, especially non-text ones (e.g., Application Insights or Elasticsearch without further configuration).

### Complexity in Customization:
- Requires familiarity with the JsonFormatter class and may involve some custom logic to handle edge cases if the log structure becomes complex.

### Less Flexible with Property Transformation:
- While you can modify top-level fields, adjusting nested properties requires more setup or custom logic in the formatter, which may complicate the formatter code.

# CustomFieldRenamingSink

## Pros:

### Complete Flexibility Over Log Event Structure:
- Provides full control over the log event, allowing you to modify not just the top-level fields but also apply custom logic to the Properties or any other part of the event.

### Supports Advanced Field Manipulation:
- You can change or filter out specific properties, flatten or restructure complex objects, and add or rename fields without the limitations of a standard formatter.

### Compatibility with Any Sink:
- Since this approach handles log events before they’re sent to a sink, it can modify the event regardless of the output sink’s requirements, making it more versatile across different targets.

## Cons:

### Additional Processing Overhead:
- Capturing, modifying, and reserializing each log event adds processing time, which may be more resource-intensive compared to a formatter.

### Increased Complexity:
- Implementing ILogEventSink requires more setup than using a formatter, especially if you need to customize the output format and serialization. Additional testing may be needed to ensure output consistency.

### Potential Formatting Duplication:
- Unlike formatters, where the custom logic is handled only at output, sinks may lead to duplicated code if you’re outputting similar log formatting to multiple targets (e.g., console and file), requiring separate setup for each sink.

# When to Use Each Approach

## Use CustomJsonTextFormatter if:
- You only need to rename or reformat top-level fields and are logging to text-based sinks like Console or File.
Performance is a high priority and you want minimal impact on the log pipeline.
You need a more lightweight solution with fewer dependencies on custom implementations.

## Use CustomFieldRenamingSink if:
- You require extensive customization of the log structure, including nested properties or conditional transformations.
You’re logging to multiple or non-standard sinks that don’t support text formatters.
Flexibility and compatibility are more important than the additional processing overhead, especially if the log volume is manageable.

# TODO

- Customization with JsonNode and Newtonsoft
- https://learn.microsoft.com/en-us/dotnet/api/system.text.json?view=net-8.0