# Custom Serilog Logging Project

This project demonstrates how to customize Serilog's JSON log output. Using a custom JSON formatter, we override Serilog’s default serialization behavior, allowing us to output logs with customized field names and formats.

# Which Approach Is Best?

Custom JSON Formatter (Approach 1) is the most direct and flexible if you're looking to customize the serialization format of Serilog log events.

Custom Enricher (Approach 2) is useful if you want to add custom fields but not necessarily suppress existing ones like Timestamp.
