using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ReservationService.Entities.Enums;

namespace ReservationService.Entities
{
    public class Reservation
    {
        public int IdReservation { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public ReservationStatus Status { get; set; }

        public int IdUser { get; set; }
        public int IdTable { get; set; }
        public Table Table { get; set; }
    }
}