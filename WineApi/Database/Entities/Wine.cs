namespace WineApi.Database.Entities
{
    public class Wine
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public required string Brand { get; set; }
        public required string Type { get; set; } 
    }
}
