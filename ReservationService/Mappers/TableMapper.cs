using ReservationService.DTO.Table;

namespace ReservationService.Mappers
{
    public static class TableMapper
    {
        public static Entities.Table ToTable(CreateTableDTO createTableDTO)
        {
            return new Entities.Table
            {
                NumberOfSeats = createTableDTO.NumberOfSeats,
                Status = createTableDTO.Status,
            };
        }

        public static TableDTO ToTableDTO(Entities.Table table)
        {
            return new TableDTO
            {
                IdTable = table.IdTable,
                NumberOfSeats = table.NumberOfSeats,
                Status = table.Status,
            };
        }
    }
}
