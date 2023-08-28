namespace Common.Entities
{
    public class ApplicationLayer
    {
        public Guid LayerId { get; private set; }
        public string LayerName { get; private set; }
        public string FilePath { get; private set; }
        public int OwnerId { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime ModifiedDate { get; private set; }
        public ApplicationUser? Owner { get; set; }

        public ApplicationLayer(Guid layerId, string layerName, string filePath, int ownerId, DateTime createDate, DateTime modifiedDate)
        {
            LayerId = layerId;
            LayerName = layerName;
            FilePath = filePath;
            OwnerId = ownerId;
            CreateDate = createDate;
            ModifiedDate = modifiedDate;
        }
    }
}
