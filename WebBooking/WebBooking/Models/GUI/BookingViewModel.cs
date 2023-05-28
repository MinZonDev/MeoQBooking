using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBooking.Models.DB;

namespace WebBooking.Models.GUI
{
    public class BookingViewModel
    {

        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumer { get; set; }
        public string IdentityID { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfPeople { get; set; }

        public int roomid { get; set; }
        public int? categoryid { get; set; }
        public string roomname { get; set; }
        public string roomdes { get; set; }
        [Column(TypeName = "money")]
        public decimal? price { get; set; }
        [StringLength(500)]
        public string image { get; set; }
        public int? maxpeople { get; set; }
        public virtual Category Category { get; set; }
        public virtual Room Room { get; set; }
    }

}