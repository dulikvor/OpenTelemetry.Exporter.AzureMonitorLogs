using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.DataModel
{
    internal static class TraceTableModelExtensionHelper
    {
        public const string NameProperty = "Name";
        public const string TraceIdProperty = "TraceId";
        public const string SpanIdProperty = "SpanId";
        public const string ParentIdProperty = "ParentId";
        public const string StartTimeProperty = "StartTime";
        public const string EndTimeProperty = "EndTime";
        public const string AttributesProperty = "Attributes";
    }
}
