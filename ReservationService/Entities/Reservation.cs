using ReservationService.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ReservationService.Entities
{
    public class Reservation
    {
        [Key]
        public int IdReservation { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        [Range(1, 50)]
        public int NumberOfGuests { get; set; }

        [Required]
        public ReservationStatus Status { get; set; }

        [Required]
        public int IdUser { get; set; }

        [Required]
        public int IdTable { get; set; }

        public Table Table { get; set; }
    }
}
