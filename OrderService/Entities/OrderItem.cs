using System;

namespace OrderService.Entities
{
    public class OrderItem
    {
        public int IdOrderItem { get; private set; }
        public int IdMenuItem { get; private set; }
        public int Quantity { get; private set; }
        public decimal PricePerItem { get; private set; }

        private OrderItem() { }

        public OrderItem(int idMenuItem, int quantity, decimal pricePerItem)
        {
            IdMenuItem = idMenuItem;
            Quantity = quantity;
            PricePerItem = pricePerItem;
        }
    }
}