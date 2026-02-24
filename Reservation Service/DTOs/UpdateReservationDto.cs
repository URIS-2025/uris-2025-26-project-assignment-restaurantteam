using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.DTOs
{
    public class UpdateReservationDto
    {
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
