using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StellumbraSite.Server.Models
{
    [Table("posts")]
    public class ForumPost
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
    }
}
