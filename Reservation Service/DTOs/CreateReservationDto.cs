using System;

namespace ReservationService.DTO
{
    public class CreateReservationDto
    {
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public int IdUser { get; set; }
        public int IdTable { get; set; }
    }
}