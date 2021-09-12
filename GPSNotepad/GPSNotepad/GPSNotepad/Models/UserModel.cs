using SQLite;

namespace GPSNotepad.Models
{
    [Table("Users")]
    public class UserModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
