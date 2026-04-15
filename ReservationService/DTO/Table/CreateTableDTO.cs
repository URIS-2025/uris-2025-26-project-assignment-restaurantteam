using ReservationService.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ReservationService.DTO.Table
{
    public class CreateTableDTO
    {
        [Required]
        [Range(1, 20)]
        public int NumberOfSeats { get; set; }

        [Required]
        public TableStatus Status { get; set; }
    }
}
