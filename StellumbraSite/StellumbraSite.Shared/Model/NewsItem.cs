using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Shared.Model
{
    [Table("news")]
    public class NewsItem
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        [ForeignKey(nameof(ForumPost))]
        [Column("post_id")]
        public int PostId { get; set; }
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