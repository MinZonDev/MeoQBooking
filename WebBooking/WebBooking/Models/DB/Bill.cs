namespace WebBooking.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bill")]
    public partial class Bill
    {
        public int billid { get; set; }

        public int bookingid { get; set; }

        public DateTime? date { get; set; }

        public int? serviceid { get; set; }

        public int? number { get; set; }

        [Column(TypeName = "money")]
        public decimal? total { get; set; }

        public int? statuscreditid { get; set; }

        public int? creditid { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalprice { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual Credit Credit { get; set; }

        public virtual Servive Servive { get; set; }

        public virtual StatusCredit StatusCredit { get; set; }
    }
}
