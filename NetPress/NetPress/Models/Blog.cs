namespace NetPress.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Blog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BlogId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(128)]
        public string UserID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateModified { get; set; }

        public int? Category { get; set; }

        public int? Status { get; set; }

        public string Content { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Category Category1 { get; set; }

        public virtual Status Status1 { get; set; }
    }
}
