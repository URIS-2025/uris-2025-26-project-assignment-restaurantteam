using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ReservationService.Entities.Enums;

namespace ReservationService.Entities
{
    public class Reservation
    {
        public int IdReservation { get; private set; }
        public DateTime ReservationDate { get; private set; }
        public int NumberOfGuests { get; private set; }
        public ReservationStatus Status { get; private set; }

        public int IdUser { get; private set; }
        public int IdTable { get; private set; }
        public Table Table { get; private set; }

        private Reservation() { }

        public Reservation(DateTime reservationDate, int numberOfGuests, int idUser, Table table)
        {
            if (table.Status == TableStatus.OCCUPIED)
                throw new InvalidOperationException("Table is already occupied.");

            if (numberOfGuests > table.NumberOfSeats)
                throw new InvalidOperationException("Number of guests exceeds table capacity.");

            ReservationDate = reservationDate;
            NumberOfGuests = numberOfGuests;
            IdUser = idUser;

            Table = table;
            IdTable = table.IdTable;

            Status = ReservationStatus.ACTIVE;
            table.Occupy();
        }

        public void Cancel()
        {
            if (Status == ReservationStatus.CANCELED)
                return;

            Status = ReservationStatus.CANCELED;
            Table.Free();
        }
    }
}