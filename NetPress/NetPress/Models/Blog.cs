namespace NetPress.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Web.Mvc;
    public partial class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(128)]
        public string UserID { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? LastModified { get; set; }

        public int? Category { get; set; }

        public int? Status { get; set; }

        [AllowHtml]
        [Required]
        public string Content { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Category CategoryObject { get; set; }

        public virtual Status StatusObject { get; set; }
    }

    public class NetPressDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
    }
}
