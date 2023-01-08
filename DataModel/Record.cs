using OpenTelemetry.Exporter.AzureMonitorLogs.Internal;
using System.Diagnostics;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.DataModel
{
    public class Record
    {
        public Record() { }
        private readonly Dictionary<string, object?> _recordValues = new();
        public Dictionary<string, object?>.KeyCollection ColumnNames => _recordValues.Keys;
        public Dictionary<string, object?> Values => _recordValues;

        public void AddValue(string columnName, ColumnType valueType, object? value)
        {
            Assert.ThrowIfNull(columnName);
            Assert.ThrowIfEqual(ColumnType.Dynamic, valueType);
            if (_recordValues.Keys.Contains(columnName))
            {
                _recordValues[columnName] = value;
                return;
            }
            _recordValues.Add(columnName, value);
        }

        public void AddValue(string columnName, ColumnType valueType, string valueName, object? value)
        {
            Assert.ThrowIfNull(columnName);
            Assert.ThrowIfNull(valueName);
            Assert.ThrowIfNotEqual(ColumnType.Dynamic, valueType);
            
            if (_recordValues.Keys.Contains(columnName))
            {
                ((Dictionary<string, object?>)_recordValues[columnName]).Add(valueName, value);
                return;
            }
            _recordValues.Add(columnName, new Dictionary<string, object?> { { valueName, value } });
        }

        public static Record Create(Activity activity)
        {
            var record = new Record();
            record.AddValue(TraceTableModelExtensionHelper.NameProperty, ColumnType.String, activity.OperationName);
            record.AddValue(TraceTableModelExtensionHelper.TraceIdProperty, ColumnType.String, activity.TraceId.ToString());
            record.AddValue(TraceTableModelExtensionHelper.SpanIdProperty, ColumnType.String, activity.SpanId.ToString());
            record.AddValue(TraceTableModelExtensionHelper.ParentIdProperty, ColumnType.String, activity.ParentSpanId.ToString());
            record.AddValue(TraceTableModelExtensionHelper.StartTimeProperty, ColumnType.DateTime, activity.StartTimeUtc);
            record.AddValue(TraceTableModelExtensionHelper.EndTimeProperty, ColumnType.DateTime, activity.StartTimeUtc + activity.Duration);
            /*activity.TagObjects.All(tag =>
            {
                record.AddValue(TraceTableModelExtensionHelper.AttributesProperty, ColumnType.Dynamic, tag.Key, tag.Value);
                return true;
            });*/

            return record;
        }
    }
}
