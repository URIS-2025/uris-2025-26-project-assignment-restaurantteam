using ReservationService.DTO.Table;

namespace ReservationService.Handlers.Table
{
    public interface ITableHandler
    {
        public Task<TableDTO> CreateTable(CreateTableDTO createTableDTO);
        public Task<List<TableDTO>> GetTables();
        public Task<TableDTO> GetTableById(int idTable);
        public Task<TableDTO> UpdateTable(UpdateTableDTO updateTableDTO, int idTable);
        public Task<bool> DeleteTable(int idTable);
    }
}
