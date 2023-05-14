using OpenTelemetry.Exporter.AzureMonitorLogs.Internal;
using System.ComponentModel.DataAnnotations;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.DataModel
{
    public enum ColumnType
    {
        String,
        Int,
        Long,
        Real,
        Boolean,
        DateTime,
        Guid,
        Dynamic
    }

    internal class ColumnModel
    {
        public string Name { get; private set; }
        public ColumnType Type { get; private set; }

        private ColumnModel(string name, ColumnType type)
        {
            Name = name;
            Type = type;
        }

        public static ColumnModel Create(string name, ColumnType type)
        {
            return new ColumnModel(name, type);
        }
    }

    internal class TableModel
    {
        private readonly Dictionary<string, TableModelValidator> _validators = new();
        private readonly List<ColumnModel> _columns = new();

        public TableModel(IEnumerable<ColumnModel> columnsCollection)
        {
            columnsCollection.All(cl => 
            {
                AddColumn(cl);
                return true;
            });
        }

        public void AddColumn(ColumnModel column)
        {
            Assert.ThrowIfNull(column);
            var conflictingColumn = _columns.FirstOrDefault(clm => clm.Name.Equals(column.Name));
            if(conflictingColumn != default)
            {
                throw new ArgumentException($"column {column.Name} already exists");
            }

            _columns.Add(column);
            _validators.Add(column.Name, TableModelValidator.Create(column.Type));
        }

        public bool ValidateRecord(Record record)
        {
            foreach(var recordKvp in record.Values)
            {
                var currentColumn = _columns.FirstOrDefault(column => column.Name.Equals(recordKvp.Key));
                if(currentColumn == default)
                {
                    throw new ArgumentException($"provided record is invalid, column {recordKvp.Key} do not exists.");
                }
                var validator = _validators[currentColumn.Name];
                if (!validator.Validate(recordKvp.Value))
                {
                    throw new ArgumentException($"provided record is invalid, column {recordKvp.Key} do not match destined column type {currentColumn.Type}.");
                }
            }

            return true;
        }
    }
}
