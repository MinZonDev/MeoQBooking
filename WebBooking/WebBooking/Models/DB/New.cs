namespace WebBooking.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("New")]
    public partial class New
    {
        public int newid { get; set; }

        public int categoryid { get; set; }

        [StringLength(250)]
        public string title { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        public DateTime? createdtime { get; set; }

        public string content { get; set; }

        [StringLength(250)]
        public string image { get; set; }

        public virtual CategoryNew CategoryNew { get; set; }
    }
}
