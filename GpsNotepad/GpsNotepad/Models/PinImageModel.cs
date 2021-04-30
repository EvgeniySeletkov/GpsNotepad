using SQLite;

namespace GpsNotepad.Models
{
    class PinImageModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int PinId { get; set; }
    }
}
