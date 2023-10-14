using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("CountryLookup", Schema = "lookups")]
    public class CountryLookup
    {
        public int CountryId { get; private set; }
        public string CountryName { get; private set; }
        public string CountryCode { get; private set; }
        public string CallingCode { get; private set; }
        public string TimeZone { get; private set; }
        public string? ISO3Code { get; private set; }
        public bool HasLevel1 { get; private set; }
        public bool HasLevel2 { get; private set; }
        public bool HasLevel3 { get; private set; }
        public bool HasLevel4 { get; private set; }
        public bool HasLevel5 { get; private set; }
        public string? Level1Name { get; private set; }
        public string? Level2Name { get; private set; }
        public string? Level3Name { get; private set; }
        public string? Level4Name { get; private set; }
        public string? Level5Name { get; private set; }

        //public List<ApplicationUserDetails>? ApplicationUsersDetails { get; set; }
        //public List<ApplicationPartnerOrganization>? ApplicationPartners { get; set; }
    }
}
