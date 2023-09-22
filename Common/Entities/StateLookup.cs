using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("PoliticalStateLookup", Schema = "lookups")]
    public class StateLookup
    {
        public int StateUniqueId { get; private set; }
        public int StateId { get; private set; }
        public string? StateCode { get; private set; }
        public string StateName { get; private set; }
        public string CountryCode { get; private set; }
    }
}
