using Common.Settings;

namespace Common.Dtos
{
    public class DapperParameter
    {
        public DapperParameter(string name, object value, DapperDbTypes dbType)
        {
            Value = value;
            Name = name;
            DbType = dbType;
        }

        public object Value { get; private set; }
        public string Name { get; private set; }
        public DapperDbTypes DbType { get; private set; }
    }
}
