namespace WebBooking.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CategoryNew")]
    public partial class CategoryNew
    {
        [Key]
        public int CategoryId { get; set; }

        [StringLength(150)]
        public string CategoryName { get; set; }
        //public virtual New New { get; set; }
    }
}
