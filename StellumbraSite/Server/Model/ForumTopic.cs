using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Server.Models
{
    [Table("topics")]
    public class ForumTopic
    {
        [Key]
        [Column("topic_name")]
        public string TopicName { get; set; }
    }
}
