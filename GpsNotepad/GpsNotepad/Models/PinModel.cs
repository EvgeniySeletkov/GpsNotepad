using SQLite;

namespace GpsNotepad.Models
{
    [Table(nameof(PinModel))]
    class PinModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        [Unique]
        public string Label { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        //rename to IsFavorite
        public bool IsFavorite { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
