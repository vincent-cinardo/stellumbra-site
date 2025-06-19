using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Shared.Model
{
    [Table("news")]
    public class NewsItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey(nameof(ForumThread))]
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
    }
}