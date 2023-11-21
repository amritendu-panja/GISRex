namespace Common.Dtos
{
    public class GetApplicationLayerResponseDto: BaseResponseDto
    {
        public Guid LayerId { get; set; }
        public string LayerName { get; set; }
        public string FilePath { get; set; }
        public int OwerId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set;}
    }
}
