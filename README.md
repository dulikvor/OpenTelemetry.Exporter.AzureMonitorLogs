# Supported .NET Versions: 
Package is targeting .NET 7 and above. 

# Background: 
AzureMonitorLogs Exporter is an open-source project that provides a simple and efficient way to export telemetry data from your application to Azure Log Analytics. 
The project is based on [open telemetry dotnet implementation](https://github.com/open-telemetry/opentelemetry-dotnet), adding a dedicated exporter targeting Log Analytics.

## Suported instrumentation
| Signal  | Status     |
| ------- | ---------- |
| Traces  | Supported |
| Logs    | Not supported |
| Metrics | Not supported |

## Supported protocols and main traits: 
### Data Collector api: 
#### Requirements for usage: 
* Azure log analytics Workspace is provisioned and known. 
* Workspace immutable id. 
* Workspace shared key, either primary or secondary. 
* Exporter will target user specified custom log table, creating it on the fly, when required. 
##### Important to note -  It is recommended not to use an existing custom log table, in order to avoid data loss.

## Getting started
### Scope decleration:
```c#
using OpenTelemetry;
using OpenTelemetry.Exporter.AzureMonitorLogs;
using OpenTelemetry.Trace;
```
### Data collector api:
#### Adding azure log analytics exporter:
```c#
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
.AddAzureMonitorLogsExporter(o =>
{
    o.WorkspaceId = //Workspace immutable id.
    o.SharedKey = //workspace shared key, can be either primary or secondary.
    o.TableName = //destination custom log table.
})
.Build();
```
## Examples:
[Console application example](https://github.com/dulikvor/OpenTelemetry.Exporter.AzureMonitorLogs/tree/main/examples/Examples.Console)
