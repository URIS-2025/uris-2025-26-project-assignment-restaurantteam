using ReservationService.Data;
using ReservationService.DTO.Table;
using ReservationService.Mappers;

namespace ReservationService.Handlers.Table
{
    public class TableHandler : ITableHandler
    {
        private readonly ReservationDbContext _context;
        public TableHandler(ReservationDbContext context) 
        { 
            _context = context;
        }

        public async Task<TableDTO> CreateTable(CreateTableDTO createTableDTO)
        {
            Entities.Table table = TableMapper.ToTable(createTableDTO);
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            TableDTO tableDTO = TableMapper.ToTableDTO(table);
            return tableDTO;
        }

        public async Task<bool> DeleteTable(int idTable)
        {
            var table = await _context.Tables.FindAsync(idTable);
            if (table == null)
            {
                throw new Exception("Table not found.");
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TableDTO>> GetTables()
        {
            var tables = _context.Tables.ToList();
            List<TableDTO> tableDTOs = new List<TableDTO>();
            foreach (var table in tables)
            {
                tableDTOs.Add(TableMapper.ToTableDTO(table));
            }
            return tableDTOs;
        }

        public async Task<TableDTO> GetTableById(int idTable)
        {
            var table = await _context.Tables.FindAsync(idTable);
            if(table == null)
            {
                throw new Exception("Table not found.");
            }
            var tableDTO = TableMapper.ToTableDTO(table);

            return tableDTO;
        }

        public async Task<TableDTO> UpdateTable(UpdateTableDTO updateTableDTO, int idTable)
        {
            var table = await _context.Tables.FindAsync(idTable);
            if (table == null)
            {
                throw new Exception("Table not found.");
            }

            if(table.NumberOfSeats != null)
                table.NumberOfSeats = updateTableDTO.NumberOfSeats;
            if (table.Status != null)
                table.Status = updateTableDTO.Status;

             _context.Tables.Update(table);
            await _context.SaveChangesAsync();

            return TableMapper.ToTableDTO(table);

        }
    }
}
