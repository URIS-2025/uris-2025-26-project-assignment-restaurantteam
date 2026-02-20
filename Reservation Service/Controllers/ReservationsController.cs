using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.DTO;
using ReservationService.Entities;

namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationDbContext _context;

        public ReservationController(ReservationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(CreateReservationDto dto)
        {
            var table = _context.Tables.Find(dto.IdTable);
            if (table == null) return NotFound("Table not found");

            var reservation = new Reservation(
                dto.ReservationDate,
                dto.NumberOfGuests,
                dto.IdUser,
                table
            );

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return Ok(reservation);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var reservations = _context.Reservations
                .Include(r => r.Table)
                .ToList();

            return Ok(reservations);
        }

        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            var reservation = _context.Reservations
                .Include(r => r.Table)
                .FirstOrDefault(r => r.IdReservation == id);

            if (reservation == null) return NotFound();

            reservation.Cancel();
            _context.SaveChanges();

            return Ok("Reservation canceled");
        }
    }
}