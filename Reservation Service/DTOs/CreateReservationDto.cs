using System;

namespace ReservationService.DTOs
{
    public class CreateReservationDto
    {
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public int IdUser { get; set; }
    }
}