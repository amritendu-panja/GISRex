namespace Common.Dtos
{
    public class CountryLookupDto
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CallingCode { get; set; }
        public bool IsStateAvailable { get; set; }
        public string StateLevelName { get; set; }
    }
}
