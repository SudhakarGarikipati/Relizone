namespace CartService.Application.Dtos
{
    public class CartItemDto
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public long CartId { get; set; }

        public string? Name { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
