using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Shared.Model
{
    [Table("user_profiles")] // match the actual PostgreSQL table name
    public class UserProfile
    {
        [Key]
        [Column("id")] // Match actual column name in PostgreSQL
        public int Id { get; set; }
        [Column("username")] // Match column exactly — PostgreSQL is case-sensitive
        public string UserName { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("role")]
        public string Role { get; set; }
    }
}
