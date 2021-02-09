namespace makingWaves.Models
{
    public class ColorModel
    {
        public int Page { get; set; }
        public int Per_page { get; set; }
        public int Total { get; set; }
        public int Total_pages { get; set; }
        public Data[] Data { get; set; }
        public Support Support { get; set; }
        public object Pantone_value { get; internal set; }
    }

}
