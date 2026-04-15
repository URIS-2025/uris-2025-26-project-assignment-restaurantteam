namespace ReservationService.DTO.Reservation
{
    public class UpdateReservationDTO
    {
        public string Status { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public int IdTable { get; set; }
    }
}
