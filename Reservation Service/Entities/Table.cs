using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using ReservationService.Entities.Enums;

namespace ReservationService.Entities
{
    public class Table
    {
        [Key]
        public int IdTable { get; set; }
        public int NumberOfSeats { get; set; }
        public TableStatus Status { get; set; }
    }
}