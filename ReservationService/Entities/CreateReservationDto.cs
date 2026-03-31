namespace ReservationService.Entities
{
    public class CreateReservationDto
    {
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public int IdTable { get; set; }
    }
}
