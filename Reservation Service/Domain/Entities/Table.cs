using ReservationService.Domain.Enums;

namespace ReservationService.Domain.Entities
{
    public class Table
    {
        public int IdTable { get; set; }
        public int NumberOfSeats { get; set; }
        public TableStatus Status { get; set; }
    }
}