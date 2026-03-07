using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationService.Controllers;
using ReservationService.Data;
using ReservationService.DTOs;
using ReservationService.Entities;
using ReservationService.Entities.Enums;
using Xunit;

namespace ReservationService.Tests
{
    public class ReservationsControllerTests
    {
        private ReservationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ReservationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ReservationDbContext(options);
        }

        private void SetUserClaims(ControllerBase controller, int userId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        // Test 1 — GetReservations vraca sve rezervacije za ADMIN
        [Fact]
        public async Task GetReservations_AsAdmin_ReturnsAllReservations()
        {
            var context = GetInMemoryContext();
            var table = new Table { NumberOfSeats = 4, Status = TableStatus.FREE };
            context.Tables.Add(table);
            await context.SaveChangesAsync();

            context.Reservations.AddRange(
                new Reservation { IdUser = 1, IdTable = table.IdTable, ReservationDate = DateTime.UtcNow, NumberOfGuests = 2, Status = ReservationStatus.ACTIVE },
                new Reservation { IdUser = 2, IdTable = table.IdTable, ReservationDate = DateTime.UtcNow, NumberOfGuests = 3, Status = ReservationStatus.ACTIVE }
            );
            await context.SaveChangesAsync();

            var controller = new ReservationsController(context);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.GetReservations();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var reservations = Assert.IsAssignableFrom<IEnumerable<ReservationResponseDto>>(okResult.Value);
            Assert.Equal(2, reservations.Count());
        }

        // Test 2 — GetReservations vraca samo svoje rezervacije za USER
        [Fact]
        public async Task GetReservations_AsUser_ReturnsOnlyOwnReservations()
        {
            var context = GetInMemoryContext();
            var table = new Table { NumberOfSeats = 4, Status = TableStatus.FREE };
            context.Tables.Add(table);
            await context.SaveChangesAsync();

            context.Reservations.AddRange(
                new Reservation { IdUser = 1, IdTable = table.IdTable, ReservationDate = DateTime.UtcNow, NumberOfGuests = 2, Status = ReservationStatus.ACTIVE },
                new Reservation { IdUser = 2, IdTable = table.IdTable, ReservationDate = DateTime.UtcNow, NumberOfGuests = 3, Status = ReservationStatus.ACTIVE }
            );
            await context.SaveChangesAsync();

            var controller = new ReservationsController(context);
            SetUserClaims(controller, 1, "CUSTOMER");

            var result = await controller.GetReservations();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var reservations = Assert.IsAssignableFrom<IEnumerable<ReservationResponseDto>>(okResult.Value);
            Assert.Single(reservations);
        }

        // Test 3 — GetReservationById vraca NotFound za nepostojeci ID
        [Fact]
        public async Task GetReservationById_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryContext();
            var controller = new ReservationsController(context);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.GetReservationById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // Test 4 — GetReservationById vraca Forbid za drugog korisnika
        [Fact]
        public async Task GetReservationById_AsOtherUser_ReturnsForbid()
        {
            var context = GetInMemoryContext();
            var table = new Table { NumberOfSeats = 4, Status = TableStatus.OCCUPIED };
            context.Tables.Add(table);
            var reservation = new Reservation { IdUser = 1, IdTable = table.IdTable, ReservationDate = DateTime.UtcNow, NumberOfGuests = 2, Status = ReservationStatus.ACTIVE };
            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();

            var controller = new ReservationsController(context);
            SetUserClaims(controller, 999, "CUSTOMER");

            var result = await controller.GetReservationById(reservation.IdReservation);

            Assert.IsType<ForbidResult>(result);
        }

        // Test 5 — CreateReservation vraca BadRequest za zauzet sto
        [Fact]
        public async Task CreateReservation_WithOccupiedTable_ReturnsBadRequest()
        {
            var context = GetInMemoryContext();
            var table = new Table { NumberOfSeats = 4, Status = TableStatus.OCCUPIED };
            context.Tables.Add(table);
            await context.SaveChangesAsync();

            var controller = new ReservationsController(context);
            SetUserClaims(controller, 1, "CUSTOMER");

            var result = await controller.CreateReservation(new CreateReservationDto
            {
                IdTable = table.IdTable,
                ReservationDate = DateTime.UtcNow,
                NumberOfGuests = 2
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Test 6 — CreateReservation vraca BadRequest ako ima previse gostiju
        [Fact]
        public async Task CreateReservation_WithTooManyGuests_ReturnsBadRequest()
        {
            var context = GetInMemoryContext();
            var table = new Table { NumberOfSeats = 2, Status = TableStatus.FREE };
            context.Tables.Add(table);
            await context.SaveChangesAsync();

            var controller = new ReservationsController(context);
            SetUserClaims(controller, 1, "CUSTOMER");

            var result = await controller.CreateReservation(new CreateReservationDto
            {
                IdTable = table.IdTable,
                ReservationDate = DateTime.UtcNow,
                NumberOfGuests = 10
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Test 7 — DeleteReservation vraca NotFound za nepostojeci ID
        [Fact]
        public async Task DeleteReservation_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryContext();
            var controller = new ReservationsController(context);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.DeleteReservation(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}