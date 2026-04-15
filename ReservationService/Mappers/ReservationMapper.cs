using ReservationService.DTO.Reservation;
using ReservationService.DTO.Table;
using ReservationService.DTO.User;

using ReservationService.Entities;

namespace ReservationService.Mappers
{
    public static class ReservationMapper
    {
        public static Entities.Reservation ToReservation(CreateReservationDTO createReservationDTO, int idUser)
        {
            return new Entities.Reservation
            {
                NumberOfGuests = createReservationDTO.NumberOfGuests,
                ReservationDate = createReservationDTO.ReservationDate,
                IdTable = createReservationDTO.IdTable,
                IdUser = idUser
            };
        }

        public static ReservationDTO ToReservationDTO(Entities.Reservation reservation)
        {
            return new ReservationDTO
            {
                IdReservation = reservation.IdReservation,
                NumberOfGuests = reservation.NumberOfGuests,
                ReservationDate = reservation.ReservationDate,
                Status = reservation.Status.ToString(),
                TableDTO = new TableDTO
                {
                    IdTable = reservation.Table.IdTable,
                    Status = reservation.Table.Status,
                    NumberOfSeats = reservation.Table.NumberOfSeats
                },
                UserDTO = new DTO.User.UserDTO
                {
                    IdUser = reservation.IdUser,
                }
            };

        }
    }
}
