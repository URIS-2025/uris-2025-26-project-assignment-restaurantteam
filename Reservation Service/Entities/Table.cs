using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ReservationService.Entities.Enums;

namespace ReservationService.Entities
{
    public class Table
    {
        public int IdTable { get; private set; }
        public int NumberOfSeats { get; private set; }
        public TableStatus Status { get; private set; }

        private Table() { }

        public Table(int numberOfSeats)
        {
            NumberOfSeats = numberOfSeats;
            Status = TableStatus.FREE;
        }

        public void Occupy()
        {
            Status = TableStatus.OCCUPIED;
        }

        public void Free()
        {
            Status = TableStatus.FREE;
        }
    }
}