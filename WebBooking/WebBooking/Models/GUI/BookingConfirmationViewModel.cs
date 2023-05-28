using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBooking.Models.GUI
{
    public class BookingConfirmationViewModel
    {
        public string RoomName { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? NumberOfPeople { get; set; }
        public decimal? Total { get; set; }
    }

}