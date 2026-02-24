using System;
using ReservationService.Entities.Enums;

namespace ReservationService.DTOs
{
    public class ReservationResponseDto
    {
        public int IdReservation { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public ReservationStatus Status { get; set; }
        public int IdUser { get; set; }
        public int IdTable { get; set; }
        public TableDto Table { get; set; }
    }

    public class TableDto
    {
        public int IdTable { get; set; }
        public int NumberOfSeats { get; set; }
        public TableStatus Status { get; set; }
    }
}