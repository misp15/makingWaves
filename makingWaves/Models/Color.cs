namespace makingWaves.Models
{
    public class Color
    {
        public Color(int id, int group, int year, string colorHex, int pantoneValue)
        {
            Id = id;
            Group = group;
            Year = year;
            ColorHex = colorHex;
            PantoneValue = pantoneValue;
        }
        public int Id { get; set; }
        public int Group { get; set; }
        public int Year { get; set; }
        public string ColorHex { get; set; }
        public int PantoneValue { get; set; }

    }

}
