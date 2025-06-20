using StellumbraSite.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StellumbraSite.Model
{
    [Table("threads")]
    public class ForumThread
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey(nameof(ForumTopic))]
        [Column("topic_name")]
        public string TopicName { get; set; }
        [Column("poster_id")]
        public string PosterId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("views")]
        public int Views { get; set; }
        [Column("datetime")]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        [ForeignKey(nameof(TopicName))]
        public ForumTopic Topic { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(PosterId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
    public class ForumThreadDto
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public string PosterId { get; set; }
        public string Title { get; set; }
        public int Views { get; set; }
    }
}
