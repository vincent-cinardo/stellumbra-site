using StellumbraSite.Data;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Model
{
    [Table("post")]
    public class ForumPost
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("thread_id")]
        public int ThreadID { get; set; }
        [Column("poster_id")]
        public string PosterId { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("is_first_post")]
        public bool IsFirstPost { get; set; }
        [Column("datetime")]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        [ForeignKey(nameof(ThreadID))]
        public ForumThread ForumThread { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(PosterId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
    public class ForumPostDto
    {
        public int Id { get; set; }
        public int ThreadID { get; set; }
        public string PosterId { get; set; }
        public string Content { get; set; }
        public bool IsFirstPost { get; set; }
    }
}