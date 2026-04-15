using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ReservationService.Handlers.Table;
using ReservationService.DTO.Table;
using ReservationService.Data;
using ReservationService.Entities;
using ReservationService.Entities.Enums;
using System.Runtime.CompilerServices;
using System.Security.Claims;


namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/tables")]
    //[Authorize]
    public class TableController : ControllerBase
    {
        private readonly ITableHandler tableHandler;

        public TableController(ITableHandler tableHandler)
        {
            this.tableHandler = tableHandler;
        }

        [HttpPost]
        public async Task<ActionResult<TableDTO>> CreateTable(CreateTableDTO createTableDTO)
        {
            var tableDTO = await tableHandler.CreateTable(createTableDTO);
            return tableDTO;
        }
        [HttpGet]
        public async Task<ActionResult<List<TableDTO>>> GetTables()
        {
            var tableDTOs = await tableHandler.GetTables();
            return tableDTOs;
        }

        [HttpGet("{idTable}")]
        public async Task<ActionResult<TableDTO>> GetTableById([FromRoute] int idTable)
        {
            var tableDTO = await tableHandler.GetTableById(idTable);
            return tableDTO;
        }

        [HttpPut("{idTable}")]
        public async Task<ActionResult<TableDTO>> UpdateTable([FromRoute] int idTable, [FromBody] UpdateTableDTO updateTableDTO)
        {
            var tableDTO = await tableHandler.UpdateTable(updateTableDTO, idTable);
            return tableDTO;
        }

        [HttpDelete("{idTable}")]
        public async Task<ActionResult<bool>> DeleteTable ([FromRoute] int idTable)
        {
            var isDeleted = await tableHandler.DeleteTable(idTable);
            return isDeleted;
        }
    }
}
