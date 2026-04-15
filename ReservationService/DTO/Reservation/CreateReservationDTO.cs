namespace ReservationService.DTO.Reservation
{
    public class CreateReservationDTO
    {
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public int IdTable { get; set; }
        public int IdUser { get; set; }
    }
}
