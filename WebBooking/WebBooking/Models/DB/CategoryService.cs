namespace WebBooking.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CategoryService")]
    public partial class CategoryService
    {
        [Key]
        public int CategoryId { get; set; }

        [StringLength(50)]
        public string CategoryName { get; set; }
    }
}
