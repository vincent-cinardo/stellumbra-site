using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Shared.Model
{

    // If this fails try ForeignKey over PostID and PosterID

    [Table("post")]
    public class ForumPost
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey(nameof(ForumThread))]
        [Column("thread_id")]
        public int ThreadID { get; set; }

        // TODO: This foreign key is wrong.
        [ForeignKey(nameof(UserProfile))]
        [Column("poster_id")]
        public string PosterID { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("is_first_thread")]
        public bool IsFirstPost { get; set; }
        [Column("datetime")]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}