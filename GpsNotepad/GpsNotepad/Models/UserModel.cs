using SQLite;

namespace GpsNotepad.Models
{
    [Table(nameof(UserModel))]
    class UserModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public string Name { get; set; }
        [Unique]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
