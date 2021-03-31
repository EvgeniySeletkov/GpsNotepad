using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNotepad.Models
{
    [Table("Users")]
    class UserModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
