using SQLite;

namespace GpsNotepad.Models.Pin
{
    [Table(nameof(PinModel))]
    class PinModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public string Label { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsFavorite { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
