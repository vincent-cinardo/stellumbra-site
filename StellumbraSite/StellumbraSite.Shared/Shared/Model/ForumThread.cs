using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Shared.Model
{

    // If this fails try ForeignKey over PostID and PosterID

    [Table("threads")]
    public class ForumThread
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey(nameof(ForumPost))]
        [Column("post_id")]
        public int PostID { get; set; }
        [ForeignKey(nameof(UserProfile))]
        [Column("poster_id")]
        public int PosterID { get; set; }
        [Column("content")]
        public string Content { get; set; }
    }
}
