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
        [ForeignKey(nameof(Topic))]
        [Column("topic_name")]
        public string TopicName { get; set; }
        [Column("title")]
        public string Title { get; set; }
    }
}
