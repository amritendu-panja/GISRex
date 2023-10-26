namespace Common.Dtos
{
    public class ApplicationOrganizationListResponseDto: BaseResponseDto
    {
        public List<BaseApplicationOrganizationListItemDto> Organizations { get; set; }
    }
}
