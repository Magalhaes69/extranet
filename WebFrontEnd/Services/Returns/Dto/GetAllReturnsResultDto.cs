namespace WebFrontEnd.Services.Returns.Dto
{
    public class GetAllReturnsResultDto
    {
        public List<GetAllReturnsResponseDto>? Items { get; set; }
        public int TotalCount { get; set; }
    }
}
