using ReservationService.Domain.Enums;

namespace ReservationService.Domain.Entities
{
    public class Reservation
    {
        public int IdReservation { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public ReservationStatus Status { get; set; }

        public int IdUser { get; set; }

        public int? IdTable { get; set; }
        public Table Table { get; set; }
    }
}