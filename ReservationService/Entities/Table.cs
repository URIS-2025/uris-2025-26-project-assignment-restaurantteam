using ReservationService.Entities.Enums;
using System.ComponentModel.DataAnnotations;

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
