using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.DTO
{
    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } // PENDING, PREPARING, READY, COMPLETED, CANCELED
    }
}