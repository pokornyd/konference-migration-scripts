namespace Konference.Models
{
    public class AssetBinary
    {
        public string FileName { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public byte[] Binary { get; set; }
    }
}
