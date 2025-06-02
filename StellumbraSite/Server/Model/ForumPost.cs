using StellumbraSite.Client.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Server.Models
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
