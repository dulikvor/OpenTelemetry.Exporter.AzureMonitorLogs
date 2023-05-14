namespace OpenTelemetry.Exporter.AzureMonitorLogs.DataModel
{
    internal abstract class TableModelValidator
    {
        public abstract bool Validate(object? value);

        public static TableModelValidator Create(ColumnType columnType)
        {
            switch(columnType)
            {
                case ColumnType.String:
                    return new ConcreteTableModelValidator<string>();
                case ColumnType.Int:
                    return new ConcreteTableModelValidator<int>();
                case ColumnType.Long:
                    return new ConcreteTableModelValidator<long>();
                case ColumnType.Real:
                    return new ConcreteTableModelValidator<decimal>();
                case ColumnType.Boolean:
                    return new ConcreteTableModelValidator<bool>();
                case ColumnType.DateTime:
                    return new ConcreteTableModelValidator<DateTime?>();
                case ColumnType.Guid:
                    return new ConcreteTableModelValidator<Guid?>();
                case ColumnType.Dynamic:
                    return new ConcreteTableModelValidator<Dictionary<string, object?>>();
                default:
                    throw new ArgumentException($"non supported column type {columnType}");
            }
        }
    }

    internal class ConcreteTableModelValidator<EType> : TableModelValidator
    {
        public override bool Validate(object? value)
        {
            return value is EType;
        }
    }
}
