using System;
using System.Collections.Generic;

namespace OrderService.Entities
{
    public enum OrderStatus
    {
        PENDING,
        PREPARING,
        READY,
        COMPLETED,
        CANCELED
    }

    public enum PaymentMethod
    {
        CASH,
        CARD
    }

    public class Order
    {
        public int IdOrder { get; private set; }
        public int IdUser { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public decimal TotalPrice { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<OrderItem> Items { get; private set; }

        private Order() { }

        public Order(int idUser, PaymentMethod paymentMethod)
        {
            IdUser = idUser;
            PaymentMethod = paymentMethod;
            OrderStatus = OrderStatus.PENDING;
            CreatedAt = DateTime.UtcNow;
            Items = new List<OrderItem>();
            TotalPrice = 0;
        }

        public void AddItem(int idMenuItem, int quantity, decimal pricePerItem)
        {
            var orderItem = new OrderItem(idMenuItem, quantity, pricePerItem);
            Items.Add(orderItem);
            RecalculateTotal();
        }

        public void UpdateStatus(OrderStatus status)
        {
            OrderStatus = status;
        }

        private void RecalculateTotal()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.Quantity * item.PricePerItem;
            }
            TotalPrice = total;
        }
    }
}