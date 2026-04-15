using ReservationService.Entities.Enums;
using ReservationService.DTO.Table;
using System.ComponentModel.DataAnnotations;
using ReservationService.DTO.User;

namespace ReservationService.DTO.Reservation
{
    public class ReservationDTO
    {
        [Key]
        public int IdReservation { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        [Range(1, 50)]
        public int NumberOfGuests { get; set; }


        public string Status { get; set; }

        public TableDTO TableDTO { get; set; }

        public UserDTO UserDTO { get; set; }
    }
}
