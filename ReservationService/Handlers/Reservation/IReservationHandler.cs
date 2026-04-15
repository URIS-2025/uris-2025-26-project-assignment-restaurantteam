using ReservationService.DTO.Reservation;

namespace ReservationService.Handlers.Reservation
{
    public interface IReservationHandler
    {
        public Task<ReservationDTO> CreateReservation(CreateReservationDTO createReservationDTO, int idUser);
        public Task<List<ReservationDTO>> GetReservations();
        public Task<ReservationDTO> GetReservationById(int idReservation);
        public Task<ReservationDTO> UpdateReservation(UpdateReservationDTO updateReservationDTO, int idReservation);
        public Task<bool> DeleteReservation(int idReservation);
    }
}
