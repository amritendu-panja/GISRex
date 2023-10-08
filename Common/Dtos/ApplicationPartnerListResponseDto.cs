namespace Common.Dtos
{
    public class ApplicationPartnerListResponseDto: BaseResponseDto
    {
        public List<BaseApplicationPartnerListItemDto> Partners { get; set; }
    }
}
