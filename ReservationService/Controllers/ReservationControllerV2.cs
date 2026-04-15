using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationService.DTO.Reservation;
using ReservationService.DTO.User;
using ReservationService.DTO.Table;
using ReservationService.Mappers;

using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.Entities;
using ReservationService.Entities.Enums;
using ReservationService.Handlers.Links;
using ReservationService.Handlers.Reservation;
using System.Security.Claims;
using ReservationService.DTO.User;
using ReservationService.Handlers.Table;

namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/v2/")]
    [Authorize]
    public class ReservationControllerV2 : ControllerBase
    {
        private readonly IReservationHandler reservationHandler;
        private readonly IUserLink userLink;
        private readonly ITableHandler tableHandler;

        public ReservationControllerV2(IReservationHandler reservationHandler,
                                        IUserLink userLink,
                                        ITableHandler tableHandler)
        {
            this.reservationHandler = reservationHandler;
            this.userLink = userLink;
            this.tableHandler = tableHandler;
        }

        [HttpPost("users/{idUser}/reservations")]
        public async Task<ActionResult<ReservationDTO>> CreateReservation(CreateReservationDTO createReservationDTO, [FromRoute] int idUser, [FromHeader] string? authorization)
        {
            UserDTO userDTO = await userLink.GetUserById(idUser, authorization);
            if (userDTO == null)
            {
                return NotFound("User not found!");
            }
            TableDTO tableDTO = await tableHandler.GetTableById(createReservationDTO.IdTable);

            if (tableDTO == null)
            {
                return NotFound("Table not found!");
            }
            if(tableDTO.Status == Entities.Enums.TableStatus.OCCUPIED)
            {
                return Conflict("Table is occupied");
            }
            ReservationDTO reservationDTO = await reservationHandler.CreateReservation(createReservationDTO, idUser);
            await tableHandler.UpdateTable(new UpdateTableDTO
            {
                NumberOfSeats = tableDTO.NumberOfSeats,
                Status = Entities.Enums.TableStatus.OCCUPIED,

            }, tableDTO.IdTable);

            reservationDTO.UserDTO = userDTO;

            return Ok(reservationDTO);
        }

        [HttpGet("reservations")]
        public async Task<ActionResult<List<ReservationDTO>>> GetReservations([FromRoute] int idUser, [FromHeader] string? authorization)
        {
            List<ReservationDTO> reservationDTOs = await reservationHandler.GetReservations();
            foreach (var reservationDTO in reservationDTOs)
            {
                UserDTO userDTO = await userLink.GetUserById(reservationDTO.UserDTO.IdUser, authorization);
                reservationDTO.UserDTO = userDTO;
            }

            return Ok(reservationDTOs);
        }

        [HttpGet("reservations/{idReservation}")]
        public async Task<ActionResult<ReservationDTO>> GetReservationById([FromRoute] int idReservation, [FromHeader] string? authorization)
        {
            ReservationDTO reservationDTO = await reservationHandler.GetReservationById(idReservation);
      

            UserDTO userDTO = await userLink.GetUserById(reservationDTO.UserDTO.IdUser, authorization);
            reservationDTO.UserDTO = userDTO;


            return Ok(reservationDTO);
        }

        [HttpPut("reservations/{idReservation}")]
        public async Task<ActionResult<ReservationDTO>> UpdateReservation([FromRoute] int idReservation, [FromHeader] string? authorization, [FromBody] UpdateReservationDTO updateReservationDTO)
        {
            ReservationDTO preReservationDTO = await reservationHandler.GetReservationById(idReservation);


            ReservationDTO reservationDTO = await reservationHandler.UpdateReservation(updateReservationDTO, idReservation);

            if(ReservationStatusParser.ToEnum(updateReservationDTO.Status) == Entities.Enums.ReservationStatus.CANCELED 
                && ReservationStatusParser.ToEnum(preReservationDTO.Status) == Entities.Enums.ReservationStatus.ACTIVE)
            {
                var updateTableDTO = new UpdateTableDTO 
                { 
                    Status = Entities.Enums.TableStatus.FREE
                };
                await tableHandler.UpdateTable(updateTableDTO, preReservationDTO.TableDTO.IdTable);
            }
            UserDTO userDTO = await userLink.GetUserById(reservationDTO.UserDTO.IdUser, authorization);
            reservationDTO.UserDTO = userDTO;


            return Ok(reservationDTO);
        }


        [HttpDelete("reservations/{idReservation}")]
        public async Task<ActionResult<bool>> DeleteReservation([FromRoute] int idReservation, [FromHeader] string? authorization)
        {
            bool isDeleted = await reservationHandler.DeleteReservation(idReservation);
            return Ok(isDeleted);
        }
    }
}
