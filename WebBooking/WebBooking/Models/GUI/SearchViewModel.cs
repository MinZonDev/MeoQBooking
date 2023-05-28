using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBooking.Models.GUI
{
    public class SearchViewModel
    {
        public string Name { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal MaxPrice { get; set; }
    }

}