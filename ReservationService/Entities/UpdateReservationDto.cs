namespace ReservationService.Entities
{
    public class UpdateReservationDto
    {
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
