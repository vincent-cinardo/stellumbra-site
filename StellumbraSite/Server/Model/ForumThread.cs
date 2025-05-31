using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Server.Models
{

    // If this fails try ForeignKey over PostID and PosterID

    [Table("threads")]
    public class ForumThread
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("post_id")]
        public string PostID { get; set; }
        [Column("poster_id")]
        public string PosterID { get; set; }
        [Column("content")] // Match column exactly — PostgreSQL is case-sensitive
        public string Content { get; set; }
    }
}
