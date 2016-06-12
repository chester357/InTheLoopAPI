namespace InTheLoopAPI.Models.Database
{
    public class StockPhoto
    {
        public int Id { get; set; }

        public string ImageURL { get; set; }

        public double HeightPx { get; set; }

        public double WidthPx { get; set; }

        public string Category { get; set; }
    }
}