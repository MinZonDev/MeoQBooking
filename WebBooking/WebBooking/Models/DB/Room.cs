namespace WebBooking.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Room")]
    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            Bookings = new HashSet<Booking>();
        }

        public int roomid { get; set; }

        public int? categoryid { get; set; }

        [StringLength(150)]
        public string roomname { get; set; }

        public string roomdes { get; set; }

        [Column(TypeName = "money")]
        public decimal? price { get; set; }

        [Column(TypeName = "money")]
        public decimal? promotion { get; set; }

        [StringLength(500)]
        public string image { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string image5 { get; set; }
        public string image6 { get; set; }
        public int sizeRoom { get; set; }

        public int? maxpeople { get; set; }
        public string view { get; set; }
        public int? bed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual Category Category { get; set; }
    }
}
