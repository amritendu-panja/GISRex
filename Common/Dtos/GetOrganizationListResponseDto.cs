namespace Common.Dtos
{
    public class GetOrganizationListResponseDto: BaseResponseDto
    {
        public List<BaseApplicationOrganizationListItemDto> Organizations { get; set; }
    }
}
