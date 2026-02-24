using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.DTO;
using ReservationService.DTOs;
using ReservationService.Entities;
using ReservationService.Entities.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    [Authorize] // Svi endpointi zahtevaju JWT
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationDbContext _context;

        public ReservationsController(ReservationDbContext context)
        {
            _context = context;
        }

        // GET: api/reservations
        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var reservations = role == "ADMIN"
                ? await _context.Reservations.Include(r => r.Table).ToListAsync()
                : await _context.Reservations
                    .Include(r => r.Table)
                    .Where(r => r.IdUser == userId)
                    .ToListAsync();

            var response = reservations.Select(r => new ReservationResponseDto
            {
                IdReservation = r.IdReservation,
                ReservationDate = r.ReservationDate,
                NumberOfGuests = r.NumberOfGuests,
                Status = r.Status,
                IdUser = r.IdUser,
                IdTable = r.IdTable,
                Table = new TableDto
                {
                    IdTable = r.Table.IdTable,
                    NumberOfSeats = r.Table.NumberOfSeats,
                    Status = r.Table.Status
                }
            });

            return Ok(response);
        }

        // GET: api/reservations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Table)
                .FirstOrDefaultAsync(r => r.IdReservation == id);

            if (reservation == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (reservation.IdUser != userId && role != "ADMIN")
                return Forbid();

            var response = new ReservationResponseDto
            {
                IdReservation = reservation.IdReservation,
                ReservationDate = reservation.ReservationDate,
                NumberOfGuests = reservation.NumberOfGuests,
                Status = reservation.Status,
                IdUser = reservation.IdUser,
                IdTable = reservation.IdTable,
                Table = new TableDto
                {
                    IdTable = reservation.Table.IdTable,
                    NumberOfSeats = reservation.Table.NumberOfSeats,
                    Status = reservation.Table.Status
                }
            };

            return Ok(response);
        }

        // POST: api/reservations
        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var table = await _context.Tables.FindAsync(dto.IdTable);
            if (table == null || table.Status != TableStatus.FREE)
                return BadRequest("Table is not available");

            var reservation = new Reservation
            {
                ReservationDate = dto.ReservationDate,
                NumberOfGuests = dto.NumberOfGuests,
                Status = ReservationStatus.ACTIVE,
                IdUser = userId,
                IdTable = dto.IdTable,
                Table = table
            };

            table.Status = TableStatus.OCCUPIED;

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.IdReservation }, reservation.IdReservation);
        }

        // PUT: api/reservations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, UpdateReservationDto dto)
        {
            var reservation = await _context.Reservations.Include(r => r.Table).FirstOrDefaultAsync(r => r.IdReservation == id);

            if (reservation == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (reservation.IdUser != userId && role != "ADMIN")
                return Forbid();

            reservation.ReservationDate = dto.ReservationDate;
            reservation.NumberOfGuests = dto.NumberOfGuests;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/reservations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.Include(r => r.Table).FirstOrDefaultAsync(r => r.IdReservation == id);

            if (reservation == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (reservation.IdUser != userId && role != "ADMIN")
                return Forbid();

            // Oslobađamo sto
            reservation.Table.Status = TableStatus.FREE;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}