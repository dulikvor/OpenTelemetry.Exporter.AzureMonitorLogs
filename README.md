# Azure Monitor Logs OpenTelemetry Exporter
[![Nuget](https://img.shields.io/badge/nuget-1.0.1-blue)](https://www.nuget.org/packages/AzureMonitorLogs.Exporter.OpenTelemetry)

## Supported .NET Versions: 
Package is targeting .NET 7 and above. 

## Background: 
AzureMonitorLogs Exporter is an open-source project that provides a simple and efficient way to export telemetry data from your application to Azure Log Analytics. 
The project is based on [open telemetry dotnet implementation](https://github.com/open-telemetry/opentelemetry-dotnet), adding a dedicated exporter targeting Log Analytics.

### Suported instrumentation
| Signal  | Status     |
| ------- | ---------- |
| Traces  | Supported |
| Logs    | Not supported |
| Metrics | Not supported |

### Supported protocols and main traits: 
#### Data Collector api: 
##### Requirements for usage: 
* Azure log analytics Workspace is provisioned and known. 
* Workspace immutable id. 
* Workspace shared key, either primary or secondary. 
* Exporter will target user specified custom log table, creating it on the fly, when required. 
###### Important to note -  It is recommended not to use an existing custom log table, in order to avoid data loss.

#### Ingestion api: 
##### Requirements for usage: 
* Azure log analytics Workspace is provisioned and known. 
* Workspace immutable id. 
* Application is provisioned and known, representing the to be instrumented service.
* Dcr and Dce.
* Application was granted the appropriate permission to Dcr.
* A log analytics custom log table was provisioned in advance, targeting open telemetry trace schema.
* Dcr stream definition depicting schema must match destination custom log table schema.
###### Important to note -  [Ingestion api tutorial on how to set Application and Permissions](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/tutorial-logs-ingestion-portal)
###### Expected custom log table schema:
| Column  | Type     |
| ------- | ---------- |
| TimeGenerated  | datetime |
| Name    | string |
| TraceId | string |
| TraceId | string |
| SpanId | string |
| ParentId | string |
| StartTime | datetime |
| EndTime | datetime |
| Attributes | dynamic |

### Getting started
#### Scope decleration:
```c#
using OpenTelemetry;
using OpenTelemetry.Exporter.AzureMonitorLogs;
using OpenTelemetry.Trace;
```
#### Data collector api:
##### Adding azure log analytics exporter:
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

#### Ingestion api:
##### Adding azure log analytics exporter:
```c#
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
.AddAzureMonitorLogsExporter(o =>
{
    o.WorkspaceId = //Workspace immutable id.
    o.ClientId = //Application client id.
    o.ClientSecret = //Application client secret.
    o.TenantId = //AAD Tenant id.
    o.AuthorityBaseUri = //AAD base url, for example - https://login.microsoftonline.com.
    o.Audience = //Azure Resource Manager target audience, for example - https://monitor.azure.com/.default
    o.DceUri = //dce uri, for example - https://yyyyyyy.{region}.ingest.monitor.azure.com
    o.DcrImmutableId = //dcr immutable id, for example - dcr-yyyyxxxxaaaaazzzz00000
    o.TableName = //destination custom log table.
})
.Build();
```
### Examples:
[Console application example](https://github.com/dulikvor/OpenTelemetry.Exporter.AzureMonitorLogs/tree/main/examples/Examples.Console)
