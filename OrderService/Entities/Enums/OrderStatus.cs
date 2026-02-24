using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Entities.Enums
{
    public enum OrderStatus
    {
        PENDING,
        PREPARING,
        READY,
        COMPLETED,
        CANCELED
    }
}
