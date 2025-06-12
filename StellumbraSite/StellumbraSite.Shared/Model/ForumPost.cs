using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Shared.Model
{
    [Table("posts")]
    public class ForumPost
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey(nameof(ForumTopic))]
        [Column("topic_name")]
        public string TopicName { get; set; }
        [Column("poster_id")]
        public string PosterID { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("datetime")]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
