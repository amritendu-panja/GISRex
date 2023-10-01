namespace Common.Dtos
{
    public class GetUsersResponseDto: BaseResponseDto
    {
        public int CurrentIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<GetUserResponseRow>? Rows { get; set; }
    }
}
