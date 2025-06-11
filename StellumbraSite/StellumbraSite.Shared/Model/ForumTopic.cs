using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Shared.Model
{
    [Table("topics")]
    public class ForumTopic
    {
        [Key]
        [Column("topic_name")]
        public string TopicName { get; set; }
        [Column("topic_shown_name")]
        public string TopicShownName { get; set; }
    }
}
