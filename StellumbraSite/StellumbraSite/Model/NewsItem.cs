using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StellumbraSite.Model
{
    [Table("news")]
    public class NewsItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("thread_id")]
        public int ThreadId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("title_image_path")]
        public string TitleImagePath { get; set; }
        [Column("caption")]
        public string Caption { get; set; }
        [Column("datetime")]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        [ForeignKey(nameof(ThreadId))]
        public ForumThread ForumThread { get; set; }
    }
    public class NewsItemDto
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string Title { get; set; }
        public string TitleImagePath { get; set; }
        public string Caption { get; set; }
    }
}