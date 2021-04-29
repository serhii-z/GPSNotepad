using SQLite;

namespace GPSNotepad.Models
{
    [Table("Pins")]
    public class PinModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsFavorit { get; set; }
        public int UserId { get; set; }
    }
}
