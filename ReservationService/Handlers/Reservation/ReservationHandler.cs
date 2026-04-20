using ReservationService.Data;
using ReservationService.DTO.Reservation;
using ReservationService.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ReservationService.Handlers.Reservation
{
    public class ReservationHandler : IReservationHandler
    {
        private readonly ReservationDbContext _context;

        public ReservationHandler(ReservationDbContext context)
        {
            _context = context;
        }

        public async Task<ReservationDTO> CreateReservation(CreateReservationDTO createReservationDTO, int idUser)
        {
            var reservation = ReservationMapper.ToReservation(createReservationDTO, idUser);
            reservation.Status = Entities.Enums.ReservationStatus.ACTIVE;
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            var reservationDTO = ReservationMapper.ToReservationDTO(reservation);
            return reservationDTO;
        }

        public async Task<bool> DeleteReservation(int idReservation)
        {
            var reservation = await _context.Reservations.Include(t => t.Table).FirstOrDefaultAsync(o => o.IdReservation == idReservation);
            if (reservation == null)
            {
                throw new Exception("Reservation not found!");
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ReservationDTO> GetReservationById(int idReservation)
        {
            Console.WriteLine("RESERVATION VBY ISS aaaa!!! " );

            var reservation = await _context.Reservations.Include(t => t.Table).FirstOrDefaultAsync(o => o.IdReservation == idReservation);

            var reservationDTO = ReservationMapper.ToReservationDTO(reservation);
            Console.WriteLine("RESERVATION aaaa!!! " + reservationDTO.IdReservation);

            return reservationDTO;
        }

        public async Task<List<ReservationDTO>> GetReservations()
        {
            var reservations = _context.Reservations.Include(t => t.Table).ToList();
            List<ReservationDTO> reservationDTOs = new List<ReservationDTO>();
            foreach (var reservation in reservations)
            {

                var reservationDTO = ReservationMapper.ToReservationDTO(reservation);
                reservationDTOs.Add(reservationDTO);
            }
            return reservationDTOs;
        }

        public async Task<ReservationDTO> UpdateReservation(UpdateReservationDTO updateReservationDTO, int idReservation)
        {
            var reservation = await _context.Reservations.Include(t => t.Table).FirstOrDefaultAsync(o => o.IdReservation == idReservation);
            if(reservation == null)
            {
                throw new Exception("Reservation not found!");
            }

            if(updateReservationDTO.Status != null)
                reservation.Status = ReservationStatusParser.ToEnum(updateReservationDTO.Status);

            if (updateReservationDTO.ReservationDate != null)
                reservation.ReservationDate = updateReservationDTO.ReservationDate.Value;

            if (updateReservationDTO.IdTable != null)
                reservation.IdTable = updateReservationDTO.IdTable.Value;

            if (updateReservationDTO.NumberOfGuests != null)
                reservation.NumberOfGuests = updateReservationDTO.NumberOfGuests.Value;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            Console.WriteLine("Ovo je sto " +reservation.Table.NumberOfSeats + " " + reservation.Table.IdTable);
            var reservationDTO = ReservationMapper.ToReservationDTO(reservation);
            return reservationDTO;
        }

        public async Task<bool> IsUserInUse(int idUser)
        {
            var reservations = await _context.Reservations.AnyAsync(o => o.IdUser == idUser);
            if (!reservations)
                return false;
            return true;
        }
    }
}
