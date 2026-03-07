using System.ComponentModel.DataAnnotations;
using ReservationService.Entities.Enums;

namespace ReservationService.Entities
{
    public class Table
    {
        [Key]
        public int IdTable { get; set; }

        [Required]
        [Range(1, 20)]
        public int NumberOfSeats { get; set; }

        [Required]
        public TableStatus Status { get; set; }
    }
}