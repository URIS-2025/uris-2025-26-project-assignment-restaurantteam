namespace OrderService.Payloads
{
    public class CreateOrderItem
    {
        public int IdMenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
    }
}
