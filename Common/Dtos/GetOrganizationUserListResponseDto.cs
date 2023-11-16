namespace Common.Dtos
{
    public class GetOrganizationUserListResponseDto : BaseResponseDto
    {
        public List<BaseOrganizationUserListItemDto> Users { get; set; }
    }
}
