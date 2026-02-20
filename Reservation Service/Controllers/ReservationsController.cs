using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.Domain.Entities;
using ReservationService.Domain.Enums;
using ReservationService.DTOs;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/reservations
        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationDto dto)
        {
            if (dto.NumberOfGuests <= 0)
                return BadRequest("Number of guests must be greater than zero.");

            var reservation = new Reservation
            {
                ReservationDate = dto.ReservationDate,
                NumberOfGuests = dto.NumberOfGuests,
                IdUser = dto.IdUser,
                Status = ReservationStatus.ACTIVE
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateReservation),
                new { id = reservation.IdReservation },
                reservation);
        }

        // DELETE: api/reservations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
                return NotFound("Reservation not found.");

            reservation.Status = ReservationStatus.CANCELED;
            reservation.IdTable = null;

            await _context.SaveChangesAsync();

            return Ok("Reservation canceled successfully.");
        }

        // GET: api/reservations/today
        [HttpGet("today")]
        public async Task<IActionResult> GetTodayReservations()
        {
            var today = DateTime.Today;

            var reservations = await _context.Reservations
                .Include(r => r.Table)
                .Where(r => r.ReservationDate.Date == today)
                .ToListAsync();

            return Ok(reservations);
        }

        // PUT: api/reservations/{id}/assign-table
        [HttpPut("{id}/assign-table")]
        public async Task<IActionResult> AssignTable(int id, AssignTableDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound("Reservation not found.");

            var table = await _context.Tables.FindAsync(dto.IdTable);
            if (table == null)
                return NotFound("Table not found.");

            if (table.Status == TableStatus.OCCUPIED)
                return BadRequest("Table is already occupied.");

            reservation.IdTable = table.IdTable;
            table.Status = TableStatus.OCCUPIED;

            await _context.SaveChangesAsync();

            return Ok("Table assigned successfully.");
        }
    }
}