namespace Common.Dtos
{
    public class CountryLookupResponseDto:BaseResponseDto
    {
        public List<CountryLookupDto> Countries { get; set; }
    }
}
